using APICatalogo.Context;
using APICatalogo.Domain.DTOs.Mappings;
using APICatalogo.Extensions;
using APICatalogo.Filters;
using APICatalogo.Repositories;
using APICatalogo.Repositories.Interfaces;
using APICatalogo.Repositories.UnitOfWork;
using APICatalogo.Services;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Adição do Swagger para documentação da API 

var mySQLConnection = builder.Configuration.GetConnectionString("DefaultConnection");

var valor1 = builder.Configuration["chave1"];
var valor2 = builder.Configuration["chave2"];
var valor3 = builder.Configuration["secao:chave1"];

builder.Services.AddDbContext<AppDbContext>(optionsAction =>
{
    optionsAction.UseMySql(mySQLConnection, ServerVersion.AutoDetect(mySQLConnection)); // Adiciona o DbContext com o MySQL
    // Para usar o SqlServer, use: optionsAction.UseSqlServer(mySQLConnection);
});

builder.Services.AddControllers(options =>
                {
                    options.Filters.Add(typeof(ApiExceptionFilter)); // Adiciona o filtro de tratamento de exceções globalmente
                }) // Adiciona os controllers
                .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); 
                // Adiciona o JsonOptions para ignorar ciclos de referência
                // (ciclos de referência são quando um objeto tem uma referência para outro objeto que tem uma referência para o primeiro objeto, causando um loop infinito)
                // Ex: Categoria possui lista de Produtos e Produto possui uma referência para Categoria, causando um loop infinito.
                // Para resolver isso, usamos o ReferenceHandler.IgnoreCycles para ignorar os ciclos de referência)

builder.Services.AddTransient<IMeuService, MeuService>();
builder.Services.AddScoped<ApiLoggingFilter>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(cfg => cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxNzg0MTYwMDAwIiwiaWF0IjoiMTc1MjY4ODgzNSIsImFjY291bnRfaWQiOiIwMTk4MTQ2NGJhYWI3OGZlYTgyYThhMDJkY2MzMTZjMCIsImN1c3RvbWVyX2lkIjoiY3RtXzAxazBhNmFmNmo3cWtha2h0cXJwMjk0MWEwIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.XQO0ni7Sq40RkeVSgwsTx9Yqubgy4jHWvOSA_zY0AFPvQV5MurHZnrNaTVOYaF5IB5aqAQHGIuSaNHVPixXs_VnxvBYyzQaRLKPoTsFfTF68eIAEBPXhAADsKOfpblf_bNcbMA-4ErNUaP3aaq2TTheMzL2WU8TBqsITVWGSytkYuw_PgPJAMFqmG6yb1gwN6Rx8lptntDehaoiI5KckA1hd27YMSDIX0WlrK9ziXAhMKTLyYmcTAIBUpTFvWr1RurP7VK8-ZJrlHU5UpMxCU3p7l7A4TgRqpddGLGIXuDbF3VTmebgB8F28QUU6UQZYOUrke6U2MsUCxbnyYVvqmQ", typeof(DTOMappingProfile));

MapsterConfig.ConfigurarMapeamento();
builder.Services.AddMapster(); // Adiciona o Mapster para mapeamento de objetos (alternativa ao AutoMapper)

var app = builder.Build();

// Configure the HTTP request pipeline.

// Configure o middleware de tratamento de erros para desenvolvimento
// A ordem dos middlewares é importante, pois eles são executados na ordem em que são adicionados

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error"); // Adição do middleware de tratamento de erros para produção
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Adição do Swagger para documentação da API
    app.UseSwaggerUI(); // Adição do SwaggerUI para documentação da API - Responde com a interface gráfica do Swagger
    app.ConfigureExceptionHandler(); // Adição do middleware de tratamento de erros para desenvolvimento
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); // Necessário para meear os controllers com as rotas

app.Run();
