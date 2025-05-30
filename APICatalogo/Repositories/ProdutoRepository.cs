using APICatalogo.Context;
using APICatalogo.Domain;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }
        public IQueryable<Produto> GetProdutos()
        {
            return _context.Produtos;
        }

        public async Task<Produto> GetProduto(int id)
        {
            var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            
            if (produto is null)
                throw new InvalidOperationException("Produto é null");

            return produto;
        }

        public async Task<Produto> Incluir(Produto produto)
        {
            if (produto is null)
                throw new ArgumentNullException(nameof(produto), "Produto não pode ser nulo");

            await _context.Produtos.AddAsync(produto);
            await _context.SaveChangesAsync();

            return produto;
        }

        public async Task<bool> Alterar(Produto produto)
        {
            if (produto is null)
                throw new ArgumentNullException(nameof(produto), "Produto não pode ser nulo");

            if (await _context.Produtos.AnyAsync(x => x.Id == produto.Id))
            {
                _context.Produtos.Update(produto);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> Excluir(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto is not null)
            {
                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
