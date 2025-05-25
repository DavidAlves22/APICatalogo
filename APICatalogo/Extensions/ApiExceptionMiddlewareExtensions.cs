using APICatalogo.Domain.DTOs;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace APICatalogo.Extensions
{
    // Classe de extensão para configurar o middleware de tratamento de exceções globalmente em DESENVOLVIMENTO
    // Traz informações detalhadas sobre o erro, como mensagem, stack trace e inner exception (se houver), por isso não é recomendado para produção.
    // Registrado em Program.cs em ambiende de desenvolvimento (app.Environment.IsDevelopment())

    public static class ApiExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {

                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                            InnerException = contextFeature.Error.InnerException != null ? contextFeature.Error.InnerException.Message : "Detalhes não disponíveis",
                            Trace = contextFeature.Error.StackTrace
                        }.ToString());
                    }
                });
            });
        }
    }
}
