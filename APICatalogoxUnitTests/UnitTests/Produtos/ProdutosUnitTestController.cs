using APICatalogo.Context;
using APICatalogo.Domain;
using APICatalogo.Domain.DTOs.Mappings;
using APICatalogo.Domain.DTOs.Produtos;
using APICatalogo.Repositories.UnitOfWork;
using AutoMapper;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace APICatalogoxUnitTests.UnitTests.Produtos;

public class ProdutosUnitTestController
{
    public IUnitOfWork unitOfWork;
    public AutoMapper.IMapper mapper;
    public MapsterMapper.IMapper mapsterMapper;
    private readonly ILogger<ProdutosUnitTestController> _logger;

    public static DbContextOptions<AppDbContext> dbContextOptions { get; }
    public static string connectionString = "Server=localhost;Port=3306;Database=ApiCatalogoDb;Uid=root;Pwd=david123;SslMode=None;";
    static ProdutosUnitTestController()
    {
        dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
           .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
           .Options;
    }
    public ProdutosUnitTestController()
    {
        var _loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new DTOMappingProfile());
        }, _loggerFactory);

        mapper = config.CreateMapper();

        var typeAdapterConfig = new TypeAdapterConfig();

        typeAdapterConfig.NewConfig<Produto, ProdutoDTO>()
            .Map(dest => dest.Nome, src => src.Nome)
            .Map(dest => dest.Descricao, src => src.Descricao)
            .Map(dest => dest.Preco, src => src.Preco)
            .Map(dest => dest.ImagemUrl, src => src.ImagemUrl)
            .Map(dest => dest.CategoriaId, src => src.CategoriaId);

        // Usando o mapper estático direto
        mapsterMapper = new MapsterMapper.Mapper(typeAdapterConfig);

        var context = new AppDbContext(dbContextOptions);
        unitOfWork = new UnitOfWork(context);
    }
}
