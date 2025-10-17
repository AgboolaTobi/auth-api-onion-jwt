using Auth.Application.DTOs;
using Auth.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _svc;
    public AuthController(IAuthService svc) => _svc = svc;

    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest req, CancellationToken ct)
    {
        try
        {
            return Ok(await _svc.RegisterAsync(req, ct));
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Username"))
        {
            return Conflict(new { field = "username", error = "Username already in use" });
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Email"))
        {
            return Conflict(new { field = "email", error = "Email already in use" });
        }
    }


    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest req, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        try
        {
            var result = await _svc.LoginAsync(req, ct);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { error = "Invalid username or password" });
        }
    }



    [Authorize]
    [HttpGet("me")]
    public ActionResult<object> Me() => Ok(new { message = "ok", user = User.Identity?.Name });
}
