namespace APICatalogo.Repositories.Interfaces
{
    public interface IRepositoryBase<T>
    {
        Task<IEnumerable<T>> GetAsync();
        Task<T> GetPorIdAsync(int id);

        // Insert, Update, Delete não são métodos assíncronos, pois não há necessidade de esperar por uma operação de banco de dados que não retorne dados.
        T Incluir(T objeto);
        bool Alterar(T objeto);
        bool Excluir(int id);
    }
}
