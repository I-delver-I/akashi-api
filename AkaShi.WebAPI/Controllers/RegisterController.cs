using AkaShi.Core.DTO.User;
using AkaShi.Core.ServiceContracts;
using AkaShi.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AkaShi.WebAPI.Controllers;

[Route("api/[controller]")]
[AllowAnonymous]
[ApiController]
public class RegisterController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public RegisterController(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }
    
    /// <summary>
    /// Create new user
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] UserRegisterDTO user)
    {
        var createdUser = await _userService.CreateUser(user);
        var token = await _authService.GenerateAccessToken(createdUser.Id, createdUser.Username, createdUser.Email);

        var result = new AuthUserDTO
        {
            User = createdUser,
            Token = token
        };

        return CreatedAtAction("GetById", "users", new { id = createdUser.Id }, result);
    }
}