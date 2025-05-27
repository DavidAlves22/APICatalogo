using APICatalogo.Context;
using APICatalogo.Domain;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categoria>> GetAsync()
        {
            var categorias = await _context.Categorias.AsNoTracking().ToListAsync();
            return categorias;
        }

        public async Task<IEnumerable<Categoria>> GetCategoriaComProdutosAsync()
        {
            var categorias = await _context.Categorias.Include(x => x.Produtos).AsNoTracking().ToListAsync();
            return categorias;
        }

        public async Task<Categoria> GetCategoriaPorId(int id)
        {
            var categorias = await _context.Categorias.FirstOrDefaultAsync(x => x.Id == id);
            return categorias;
        }

        public async Task<Categoria> Incluir(Categoria categoria)
        {
            if (categoria is null)
                throw new ArgumentNullException(nameof(categoria), "Categoria não pode ser nula.");

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return categoria;
        }

        public async Task<Categoria> Alterar(Categoria categoria)
        {
            if (categoria is null)
                throw new ArgumentNullException(nameof(categoria), "Categoria não pode ser nula.");

            if (categoria.Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(categoria.Id), "Id deve ser maior que zero.");

            _context.Entry(categoria).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return categoria;
        }

        public async Task<Categoria> Excluir(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id deve ser maior que zero.");

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria is null)
                throw new KeyNotFoundException($"Categoria com Id {id} não encontrada.");

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return categoria;
        }
    }
}
