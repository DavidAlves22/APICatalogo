using APICatalogo.Context;
using APICatalogo.Domain;
using APICatalogo.Domain.DTOs.Produtos;
using APICatalogo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    public class ProdutoRepository : RepositoryBase<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Produto>> GetComPaginacaoAsync(ParametrosProduto parametros)
        {
            return await _context.Set<Produto>().AsNoTracking()
                        .OrderBy(or => or.Nome)
                        .Skip((parametros.PageNumber - 1) * parametros.PageSize)
                        .Take(parametros.PageSize).ToListAsync();
        }
    }
}
