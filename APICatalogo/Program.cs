using APICatalogo.Context;
using APICatalogo.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Adição do Swagger para documentação da API 

var mySQLConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(optionsAction =>
{
    optionsAction.UseMySql(mySQLConnection, ServerVersion.AutoDetect(mySQLConnection)); // Adiciona o DbContext com o MySQL
    // Para usar o SqlServer, use: optionsAction.UseSqlServer(mySQLConnection);
});

builder.Services.AddControllers() // Adiciona os controllers
                .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); 
                // Adiciona o JsonOptions para ignorar ciclos de referência
                // (ciclos de referência são quando um objeto tem uma referência para outro objeto que tem uma referência para o primeiro objeto, causando um loop infinito)
                // Ex: Categoria possui lista de Produtos e Produto possui uma referência para Categoria, causando um loop infinito.
                // Para resolver isso, usamos o ReferenceHandler.IgnoreCycles para ignorar os ciclos de referência)

builder.Services.AddTransient<IMeuService, MeuService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Adição do Swagger para documentação da API
    app.UseSwaggerUI(); // Adição do SwaggerUI para documentação da API - Responde com a interface gráfica do Swagger
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); // Necessário para meear os controllers com as rotas

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error"); // Adição do middleware de tratamento de erros para produção
}

app.Run();
