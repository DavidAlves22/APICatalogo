using APICatalogo.Domain;
using APICatalogo.Domain.DTOs.Produtos;

namespace APICatalogo.Repositories.Interfaces
{
    public interface IProdutoRepository : IRepositoryBase<Produto>
    {
        Task<IEnumerable<Produto>> GetComPaginacaoAsync(ParametrosProduto parametros);
    }
}
