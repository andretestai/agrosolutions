using IdentityService.DTOs;
using IdentityService.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest req)
    {
        var result = _authService.Register(req);

        if (result is null)
            return Conflict(new { message = "E-mail já cadastrado." });

        return Created(string.Empty, result);
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest req)
    {
        var result = _authService.Login(req);

        if (result is null)
            return Unauthorized(new { message = "Credenciais inválidas." });

        return Ok(result);
    }
}