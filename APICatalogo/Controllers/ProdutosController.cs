using APICatalogo.Context;
using APICatalogo.Domain;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _context.Produtos?.ToList();

            if (produtos == null)
                return NotFound("Produtos não encontrados");

            return produtos;
        }

        [HttpGet("{id:int}", Name="ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos?.FirstOrDefault(x => x.Id == id);

            if (produto is null)
                return NotFound("Produto não encontrado");

            return produto;
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
                return BadRequest("Produto inválido");

            _context.Produtos?.Add(produto);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = produto.Id }, produto);
        }

        [HttpPut]
        public ActionResult Put(Produto produto)
        {
            if (produto is null)
                return BadRequest("Produto inválido");
            if (produto.Id == 0)
                return BadRequest("Produto inválido");
            _context.Entry(produto).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos?.FirstOrDefault(x => x.Id == id);
            if (produto is null)
                return NotFound("Produto não encontrado");

            _context.Produtos?.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }
    }
}
