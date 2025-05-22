using APICatalogo.Context;
using APICatalogo.Domain;
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

        [HttpGet()]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _context.Categorias?.ToList();
            if (categorias == null)
                return NotFound("Categorias não encontradas");
            return categorias;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriaComProdutos()
        {
            var categorias = _context.Categorias?.Include(x=>x.Produtos).ToList();
            if (categorias == null)
                return NotFound("Categorias não encontradas");

            return categorias;
        }
    }
}
