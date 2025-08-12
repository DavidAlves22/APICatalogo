using APICatalogo.Domain;
using APICatalogo.Domain.DTOs.Autenticacao;
using APICatalogo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName!);
            if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password!))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim("id", user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = _tokenService.GenerateAcessToken(authClaims, _configuration);
                var refreshToken = _tokenService.GenerateRefreshToken();
                user.RefreshToken = refreshToken;

                _ = int.TryParse(_configuration["Jwt:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(refreshTokenValidityInMinutes);

                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                });
            }

            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.UserName!);

            if (userExists is not null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Usuário já existe!" });
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName
            };

            var result = await _userManager.CreateAsync(user, model.Password!);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = $"Erro ao criar usuário! {string.Join("; ", result.Errors.Select(i => i.Description))}" });
            }

            return Ok(new { Status = "Success", Message = "Usuário criado com sucesso!" });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string? acessToken = tokenModel.AccessToken ?? throw new ArgumentNullException(nameof(tokenModel));
            string? refreshToken = tokenModel.RefreshToken ?? throw new ArgumentNullException(nameof(tokenModel));

            var principal = _tokenService.GetPrincipalFromExpiredToken(acessToken!, _configuration);
            if (principal is null)
            {
                return BadRequest("Invalid access token/refresh");
            }

            var user = await _userManager.FindByNameAsync(principal.Identity!.Name!);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid token or user not found or refresh token invalid");
            }

            var novoToken = _tokenService.GenerateAcessToken(principal.Claims.ToList(), _configuration);
            var novoRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = novoRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:RefreshTokenValidityInMinutes"]!));
            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(novoToken),
                refreshToken = novoRefreshToken,
                Expiration = novoToken.ValidTo
            });
        }

        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user is null)
                return BadRequest("Invalid user name");

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            return NoContent();
        }

        [HttpPost]
        [Route("create-role")]
        [Authorize (Policy = "SuperAdminOnly")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Role já existe!" });

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = $"Erro ao criar role! {string.Join("; ", result.Errors.Select(i => i.Description))}" });

            _logger.LogInformation($"Role {roleName} criada com sucesso.");
            return Ok(new { Status = "Success", Message = "Role criada com sucesso!" });
        }

        [HttpPost]
        [Route("add-user-role")]
        [Authorize(Policy = "SuperAdminOnly")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Usuário não existe!" });

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Role não existe!" });

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = $"Erro ao adicionar role ao usuário! {string.Join("; ", result.Errors.Select(i => i.Description))}" });

            return Ok(new { Status = "Success", Message = "Role adicionada ao usuário com sucesso!" });
        }
    }
}
