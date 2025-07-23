using APICatalogo.Domain.DTOs.Categorias;
using APICatalogo.Domain.DTOs.Produtos;
using AutoMapper;

namespace APICatalogo.Domain.DTOs.Mappings
{
    public class DTOMappingProfile : Profile
    {
        public DTOMappingProfile()
        {
            CreateMap<Produto, ProdutoDTO>().ReverseMap();
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
        }
    }
}
