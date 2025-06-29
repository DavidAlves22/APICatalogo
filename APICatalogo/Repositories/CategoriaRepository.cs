using APICatalogo.Context;
using APICatalogo.Domain;
using APICatalogo.Repositories.Interfaces;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository : RepositoryBase<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }
    }
}
