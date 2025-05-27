using APICatalogo.Domain;

namespace APICatalogo.Repositories
{
    public interface ICategoriaRepository
    {
        public Task<IEnumerable<Categoria>> GetAsync();
        public Task<Categoria> GetCategoriaPorId(int id);
        public Task<IEnumerable<Categoria>> GetCategoriaComProdutosAsync();
        public Task<Categoria> Incluir(Categoria categoria);
        public Task<Categoria> Alterar(Categoria categoria);
        public Task<Categoria> Excluir(int id);
    }
}
