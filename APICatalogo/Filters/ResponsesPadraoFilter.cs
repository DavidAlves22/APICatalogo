using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;

namespace APICatalogo.Filters
{
    public class ResponsesPadraoFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Responses.TryAdd(((int)HttpStatusCode.OK).ToString(),
            new OpenApiResponse { Description = "OK – Requisição bem-sucedida." });

            operation.Responses.TryAdd(((int)HttpStatusCode.Created).ToString(),
                new OpenApiResponse { Description = "Created – Recurso criado com sucesso." });

            operation.Responses.TryAdd(((int)HttpStatusCode.NoContent).ToString(),
                new OpenApiResponse { Description = "No Content – Sem conteúdo no corpo da resposta." });

            operation.Responses.TryAdd(((int)HttpStatusCode.BadRequest).ToString(),
                new OpenApiResponse { Description = "Bad Request – Requisição inválida." });

            operation.Responses.TryAdd(((int)HttpStatusCode.Unauthorized).ToString(),
                new OpenApiResponse { Description = "Unauthorized – Autenticação necessária ou falhou." });

            operation.Responses.TryAdd(((int)HttpStatusCode.Forbidden).ToString(),
                new OpenApiResponse { Description = "Forbidden – Sem permissão para acessar o recurso." });

            operation.Responses.TryAdd(((int)HttpStatusCode.NotFound).ToString(),
                new OpenApiResponse { Description = "Not Found – Recurso não encontrado." });

            operation.Responses.TryAdd(((int)HttpStatusCode.InternalServerError).ToString(),
                new OpenApiResponse { Description = "Internal Server Error – Erro interno no servidor." });
        }
    }
}
