using APICatalogo.Context;
using APICatalogo.Repositories.Interfaces;

namespace APICatalogo.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IProdutoRepository _produtoRepository;

        private ICategoriaRepository _categoriaRepository;

        public AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        // Lazy loading of repositories para evitar a criação de instâncias desnecessárias
        public IProdutoRepository ProdutoRepository
        {
            get
            {
                return _produtoRepository ?? new ProdutoRepository(_context);
            }
        }

        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                return _categoriaRepository ?? new CategoriaRepository(_context);
            }
        }

        // Até aqui Lazy loading of repositories para evitar a criação de instâncias desnecessárias

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
