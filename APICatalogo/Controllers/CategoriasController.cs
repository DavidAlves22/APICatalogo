using APICatalogo.Context;
using APICatalogo.Domain;
using APICatalogo.Filters;
using APICatalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public CategoriasController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("valores-appsettings")]
        public ActionResult<string> GetValoresAppSettings()
        {
            var valor1 = _configuration["chave1"];
            var valor2 = _configuration["chave2"];
            var valor3 = _configuration["secao:chave1"];
            return $"Chave 1: {valor1} \nChave 2: {valor2} \nChave 3: {valor3}";
        }

        [HttpGet("Injetando-Services-No-Escopo")]
        public ActionResult<string> GetMensagemFromServices(IMeuService meuServico, string nome) // Injetando serviço no escopo do controlador, não sendo necessário o uso do construtor.
        {
            return meuServico.GetMensagem(nome);
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))] // Usando o filtro de logging para registrar as informações da requisição e resposta
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAsync()
        {            
            // Simulando um erro para testar o middleware de tratamento de exceções.
            //throw new Exception("Erro ao tentar recuperar categorias"); 
            try
            {

                var categorias = await _context.Categorias.AsNoTracking().ToListAsync(); // Usar AsNoTracking para melhorar a performance em consultas de leitura mas se precisar persistir as entidades, não use.
                if (categorias == null)
                    return NotFound("Categorias não encontradas");
                return categorias;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar categorias. Erro: {ex.Message}");
            }
        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriaComProdutosAsync()
        {
            try
            {
                var categorias = await _context.Categorias.AsNoTracking().Include(x => x.Produtos).ToListAsync();
                if (categorias == null)
                    return NotFound("Categorias não encontradas");

                return categorias;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar categorias. Erro: {ex.Message}");
            }
        }
    }
}
