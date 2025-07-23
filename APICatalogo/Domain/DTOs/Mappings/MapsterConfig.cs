using Mapster;

namespace APICatalogo.Domain.DTOs.Mappings
{
    public class MapsterConfig
    {
        public static void ConfigurarMapeamento()
        {
            TypeAdapterConfig<Produto, ProdutoDTO>.NewConfig()
                .Map(dest => dest.Nome, src => src.Nome)
                .Map(dest => dest.Descricao, src => src.Descricao)
                .Map(dest => dest.Preco, src => src.Preco)
                .Map(dest => dest.ImagemUrl, src => src.ImagemUrl)
                .Map(dest => dest.CategoriaId, src => src.CategoriaId);
        }
    }
}
