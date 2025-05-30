using APICatalogo.Domain;

namespace APICatalogo.Repositories
{
    public interface IProdutoRepository
    {
        IQueryable<Produto> GetProdutos();
        Task<Produto> GetProduto(int id);
        Task<Produto> Incluir(Produto produto);
        Task<bool> Alterar(Produto produto);
        Task<bool> Excluir(int id);
    }
}
