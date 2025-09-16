using APICatalogo.Context;
using APICatalogo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly AppDbContext _context;

        public RepositoryBase(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> GetPorIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public T Incluir(T objeto)
        {
            _context.Set<T>().Add(objeto);
            _context.SaveChanges();
            return objeto;
        }

        public bool Alterar(T objeto)
        {
            try
            {
                _context.Set<T>().Update(objeto);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }            
        }

        public bool Excluir(int id)
        {
            var objeto = _context.Set<T>().Find(id);

            if (objeto is not null)
            {
                _context.Set<T>().Remove(objeto);
                return true;
            }

            return false;
        }
    }
}
