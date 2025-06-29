using APICatalogo.Repositories.Interfaces;

namespace APICatalogo.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        IProdutoRepository ProdutoRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }
        Task CommitAsync();
    }
}
