using APICatalogo.Context;
using APICatalogo.Domain;
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

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("Injetando-Services-No-Escopo")]
        public ActionResult<string> GetMensagemFromServices(IMeuService meuServico, string nome) // Injetando serviço no escopo do controlador, não sendo necessário o uso do construtor.
        {
            return meuServico.GetMensagem(nome);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAsync()
        {
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
