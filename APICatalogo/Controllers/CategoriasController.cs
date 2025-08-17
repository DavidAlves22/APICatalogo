using APICatalogo.Domain;
using APICatalogo.Domain.DTOs.Categorias;
using APICatalogo.Domain.DTOs.Mappings;
using APICatalogo.Filters;
using APICatalogo.Repositories.Interfaces;
using APICatalogo.Repositories.UnitOfWork;
using APICatalogo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        public CategoriasController(IConfiguration configuration, ILogger<CategoriasController> logger, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _logger = logger;
            _unitOfWork = unitOfWork;
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

        /// <summary>
        /// Obtém uma lista de categorias.
        /// </summary>
        /// <returns>Objetos Categoria</returns>
        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))] 
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAsync()
        {
            // Simulando um erro para testar o middleware de tratamento de exceções.
            //throw new Exception("Erro ao tentar recuperar categorias"); 
            var categorias = await _unitOfWork.CategoriaRepository.GetAsync();
            if (categorias == null)
                return NotFound("Categorias não encontradas");

            var categoriasDTO = categorias.ToCategoriaDTOList(); 

            return Ok(categoriasDTO);
        }

        /// <summary>
        /// Obtém uma categoria por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objetos Categoria</returns>
        [HttpGet("{id:int}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriaPorId(int id)
        {
            var categoria = await _unitOfWork.CategoriaRepository.GetPorIdAsync(id);
            if (categoria == null)
                return NotFound("Categoria não encontrada");

            var categoriaDTO = categoria.ToCategoriaDTO(); // Usando o método de extensão para converter a categoria para DTO

            return Ok(categoriaDTO);
        }


        /// <summary>
        /// Inclui uma nova categoria.
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        ///     POST api/categorias{
        ///     "categoriaId": 0,
        ///     "nome": "Categoria Teste",
        ///     "imagemUrl": "https://example.com/imagem.jpg"
        /// }
        /// </remarks>
        /// <param name="categoriaDTO"></param>
        /// <returns>O objeto Categoria incluído</returns>
        /// <remarks>Retorna um objeto Categoria incluído</remarks>
        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> Incluir([FromBody] CategoriaDTO categoriaDTO)
        {
            _logger.LogInformation("#### POST api/categorias"); // Logando a requisição no console
            if (categoriaDTO == null)
                return BadRequest("Categoria não pode ser nula.");

            var categoria = categoriaDTO.ToCategoria();

            var categoriaCriada = _unitOfWork.CategoriaRepository.Incluir(categoria);
            await _unitOfWork.CommitAsync();

            var novaCategoriaDTO = categoriaCriada.ToCategoriaDTO();

            return CreatedAtAction(nameof(GetCategoriaPorId), new { id = novaCategoriaDTO.Id }, novaCategoriaDTO);
        }

        [HttpPut()]
        public async Task<ActionResult<CategoriaDTO>> Alterar([FromBody] CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO == null || categoriaDTO.Id <= 0)
                return BadRequest("Categoria inválida ou Id não corresponde.");

            var categoria = categoriaDTO.ToCategoria();

            var categoriaExistente = _unitOfWork.CategoriaRepository.Alterar(categoria);
            if (!categoriaExistente)
                return NotFound("Categoria não encontrada.");

            await _unitOfWork.CommitAsync();

            return Ok(categoriaDTO);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<CategoriaDTO>> Excluir(int id)
        {
            var categoriaExcluida = _unitOfWork.CategoriaRepository.Excluir(id);

            if (!categoriaExcluida)
                return NotFound("Categoria não encontrada.");

            await _unitOfWork.CommitAsync();

            return Ok(categoriaExcluida);
        }
    }
}
