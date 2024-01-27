using AkaShi.Core.DTO.User;
using AkaShi.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AkaShi.WebAPI.Controllers;

[Route("api/[controller]")]
[AllowAnonymous]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    /// <summary>
    /// Generates user token for authorization
    /// </summary>
    /// /// <remarks>
    /// Sample request with existing user:
    ///
    ///     POST /api/auth/login
    ///     {
    ///        "email": "test@gmail.com",
    ///        "password": "passw0rd"
    ///     }
    ///
    /// </remarks>
    [HttpPost("login")]
    public async Task<ActionResult<AuthUserDTO>> Login(UserLoginDTO dto)
    {
        return Ok(await _authService.Authorize(dto));
    }
}