using APICatalogo.Context;
using APICatalogo.Domain;
using APICatalogo.Repositories.Interfaces;

namespace APICatalogo.Repositories
{
    public class ProdutoRepository : RepositoryBase<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }    
    }
}
