using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[ApiController]
[Route("api/teste")]
[ApiVersion("3.0")]
[ApiVersion("4.0")]
public class TesteV3Controller : ControllerBase
{
    [MapToApiVersion("3.0")]
    [HttpGet]
    public string GetVersion3()
    {
        return "Teste V3 - Versão 3.0.0";
    }

    [MapToApiVersion("4.0")]
    [HttpGet]
    public string GetVersionV4()
    {
        return "Teste V3 - Versão 4.0.0";
    }
}
