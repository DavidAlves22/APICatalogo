using APICatalogo.Domain;
using APICatalogo.Filters;
using APICatalogo.Repositories.Interfaces;
using APICatalogo.Repositories.UnitOfWork;
using APICatalogo.Services;
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

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))] // Usando o filtro de logging para registrar as informações da requisição e resposta
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAsync()
        {
            // Simulando um erro para testar o middleware de tratamento de exceções.
            //throw new Exception("Erro ao tentar recuperar categorias"); 
            var categorias = await _unitOfWork.CategoriaRepository.GetAsync();
            if (categorias == null)
                return NotFound("Categorias não encontradas");

            return Ok(categorias);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriaPorId(int id)
        {
            var categorias = await _unitOfWork.CategoriaRepository.GetPorIdAsync(id);
            if (categorias == null)
                return NotFound("Categoria não encontrada");

            return Ok(categorias);
        }

        [HttpPost]
        public async Task<ActionResult<Categoria>> Incluir([FromBody] Categoria categoria)
        {
            _logger.LogInformation("#### POST api/categorias"); // Logando a requisição no console
            if (categoria == null)
                return BadRequest("Categoria não pode ser nula.");
            var categoriaCriada = await _unitOfWork.CategoriaRepository.Incluir(categoria);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(nameof(GetCategoriaPorId), new { id = categoriaCriada.Id }, categoriaCriada);
        }

        [HttpPut()]
        public async Task<ActionResult<Categoria>> Alterar([FromBody] Categoria categoria)
        {
            if (categoria == null || categoria.Id != categoria.Id)
                return BadRequest("Categoria inválida ou Id não corresponde.");

            var categoriaExistente = await _unitOfWork.CategoriaRepository.Alterar(categoria);
            if (!categoriaExistente)
                return NotFound("Categoria não encontrada.");

            await _unitOfWork.CommitAsync();

            return Ok(categoriaExistente);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Categoria>> Excluir(int id)
        {
            var categoriaExcluida = await _unitOfWork.CategoriaRepository.Excluir(id);

            if (!categoriaExcluida)
                return NotFound("Categoria não encontrada.");

            await _unitOfWork.CommitAsync();

            return Ok(categoriaExcluida);
        }
    }
}
