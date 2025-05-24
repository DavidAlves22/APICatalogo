using APICatalogo.Context;
using APICatalogo.Extensions;
using APICatalogo.Filters;
using APICatalogo.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Adi��o do Swagger para documenta��o da API 

var mySQLConnection = builder.Configuration.GetConnectionString("DefaultConnection");

var valor1 = builder.Configuration["chave1"];
var valor2 = builder.Configuration["chave2"];
var valor3 = builder.Configuration["secao:chave1"];

builder.Services.AddDbContext<AppDbContext>(optionsAction =>
{
    optionsAction.UseMySql(mySQLConnection, ServerVersion.AutoDetect(mySQLConnection)); // Adiciona o DbContext com o MySQL
    // Para usar o SqlServer, use: optionsAction.UseSqlServer(mySQLConnection);
});

builder.Services.AddControllers() // Adiciona os controllers
                .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); 
                // Adiciona o JsonOptions para ignorar ciclos de refer�ncia
                // (ciclos de refer�ncia s�o quando um objeto tem uma refer�ncia para outro objeto que tem uma refer�ncia para o primeiro objeto, causando um loop infinito)
                // Ex: Categoria possui lista de Produtos e Produto possui uma refer�ncia para Categoria, causando um loop infinito.
                // Para resolver isso, usamos o ReferenceHandler.IgnoreCycles para ignorar os ciclos de refer�ncia)

builder.Services.AddTransient<IMeuService, MeuService>();
builder.Services.AddScoped<ApiLoggingFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.

// Configure o middleware de tratamento de erros para desenvolvimento
// A ordem dos middlewares � importante, pois eles s�o executados na ordem em que s�o adicionados

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error"); // Adi��o do middleware de tratamento de erros para produ��o
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Adi��o do Swagger para documenta��o da API
    app.UseSwaggerUI(); // Adi��o do SwaggerUI para documenta��o da API - Responde com a interface gr�fica do Swagger
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); // Necess�rio para meear os controllers com as rotas

app.Run();
