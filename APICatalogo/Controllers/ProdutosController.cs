using APICatalogo.Context;
using APICatalogo.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        //IActionResult - Retorna o resultado da ação, independente do tipo de retorno (Precisa retornar uma ActionResult sempre)
        //ActionResult - Retorna o resultado da ação, mas não importa o tipo de retorno (pode ser um objeto ou uma string)
        //ActionResult<T> - Retorna o resultado da ação, mas importa o tipo de retorno (pode ser um objeto ou uma string)
        //T - Tipo de retorno da ação (pode ser um objeto ou uma string)

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
        {
            try
            {
                var produtos = await _context.Produtos.AsNoTracking().ToListAsync();

                if (produtos == null)
                    return NotFound("Produtos não encontrados");

                return produtos;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar categorias. Erro: {ex.Message}");
            }
        }

        //{id:int:length(2)} - O id deve ser um inteiro e ter 2 dígitos (Existem outras formas de restringir o id, como por exemplo: {id:int:min(1)} - O id deve ser um inteiro e maior que 1)
        [HttpGet("{id:int:length(2)}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> GetAsync(int id, [BindRequired] string nome) //[BindRequired] exige que o atributo nome seja obrigatório
        {
            try
            {
                var nomeProduto = nome;
                var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                if (produto is null)
                    return NotFound("Produto não encontrado");

                return produto;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar categorias. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult<Produto> Post(Produto produto)
        {
            try
            {
                if (produto is null)
                    return BadRequest("Produto inválido");

                _context.Produtos?.Add(produto);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetAsync), new { id = produto.Id }, produto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar categorias. Erro: {ex.Message}");
            }
        }

        [HttpPut]
        public ActionResult<Produto> Put(Produto produto)
        {
            try
            {
                if (produto is null)
                    return BadRequest("Produto inválido");
                if (produto.Id == 0)
                    return BadRequest("Produto inválido");
                _context.Entry(produto).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();

                return produto;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar categorias. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var produto = _context.Produtos?.FirstOrDefault(x => x.Id == id);
                if (produto is null)
                    return NotFound("Produto não encontrado");

                _context.Produtos?.Remove(produto);
                _context.SaveChanges();

                return Ok(produto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar categorias. Erro: {ex.Message}");
            }
        }
    }
}
