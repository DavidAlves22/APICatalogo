using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filters
{
    // Filtro de ação para registrar informações da requisição e resposta da API
    // Implementa a interface IActionFilter para registrar informações antes e depois da execução da ação
    // Para registrar o filtro, adicione o atributo [ServiceFilter(typeof(ApiLoggingFilter))] na ação ou no controlador

    public class ApiLoggingFilter : IActionFilter
    {
        protected readonly ILogger<ApiLoggingFilter> _logger;

        public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Executing action: {ActionName}", context.ActionDescriptor.DisplayName);
            _logger.LogInformation("##################################################");
            _logger.LogInformation($"Data: {DateTime.Now.ToString("dd-MM-yyyy HH:mm")}");            
            _logger.LogInformation($"Model State: {context.ModelState.IsValid}");
            _logger.LogInformation("##################################################");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Executing action: {ActionName}", context.ActionDescriptor.DisplayName);
            _logger.LogInformation("##################################################");
            _logger.LogInformation($"Data: {DateTime.Now.ToString("dd-MM-yyyy HH:mm")}");
            _logger.LogInformation($"Status Code: {context.HttpContext.Response.StatusCode}");
            _logger.LogInformation("##################################################");
        }
    }
}
