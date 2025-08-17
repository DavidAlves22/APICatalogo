using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("api/v{version:apiVersion}/teste")]
[ApiController]
[ApiVersion("1.0", Deprecated = true)]
public class TesteV1Controller : ControllerBase
{
    [HttpGet]
    public string GetVersion()
    {
        return "Teste V1 - Versão 1.0.0";
    }
}
