namespace APICatalogo.Repositories.Interfaces
{
    public interface IRepositoryBase<T>
    {
        Task<IEnumerable<T>> GetAsync();
        Task<T> GetPorIdAsync(int id);
        Task<T> Incluir(T objeto);
        Task<bool> Alterar(T objeto);
        Task<bool> Excluir(int id);
    }
}
