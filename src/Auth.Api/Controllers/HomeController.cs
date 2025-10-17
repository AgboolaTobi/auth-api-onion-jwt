using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet("/")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Get() => Ok(new { service = "Auth.Api", status = "OK" });
}
