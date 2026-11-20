using Microsoft.AspNetCore.Mvc;
using TicketeraLab10.Application.DTOs;
using TicketeraLab10.Application.UseCases.Auth;

namespace TicketeraLab10.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IRegisterUseCase _register;
    private readonly ILoginUseCase _login;

    public AuthController(IRegisterUseCase register, ILoginUseCase login)
    {
        _register = register;
        _login = login;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        try { return Ok(await _register.ExecuteAsync(dto)); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        try { return Ok(await _login.ExecuteAsync(dto)); }
        catch (UnauthorizedAccessException ex) { return Unauthorized(new { message = ex.Message }); }
    }
}