using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filters
{
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
