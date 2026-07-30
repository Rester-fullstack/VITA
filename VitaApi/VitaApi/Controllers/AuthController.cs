using Microsoft.AspNetCore.Mvc;

using VitaApi.DTOs.Auth;
using VitaApi.Services;

namespace VitaApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(
        AuthService authService
    )
    {
        _authService = authService;
    }


    [HttpPost("register")]
    public async Task<IActionResult>
        Register(RegisterDto dto)
    {
        var result =
            await _authService.Register(dto);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return StatusCode(201, result);
    }

  

    [HttpPost("login")]
    public async Task<IActionResult>
        Login(LoginDto dto)
    {
        var result =
            await _authService.Login(dto);

        if (!result.Success)
        {
            return Unauthorized(result);
        }

        return Ok(result);
    }



    [HttpPost("refresh")]
    public async Task<IActionResult>
        RefreshToken(
            RefreshTokenRequestDto dto
        )
    {
        var result =
            await _authService
                .RefreshToken(dto);

        if (!result.Success)
        {
            return Unauthorized(result);
        }

        return Ok(result);
    }

  

    [HttpPost("logout")]
    public async Task<IActionResult>
        Logout(
            RefreshTokenRequestDto dto
        )
    {
        var result =
            await _authService
                .Logout(dto.RefreshToken);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return NoContent();
    }
}