namespace APICatalogo.Domain.DTOs.Produtos
{
    public class ParametrosProduto
    {
        const int MaxPageSize = 10;
        public int PageNumber { get; set; } = 1;
        private int _pageSize { get; set; }
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
