using AkaShi.Core.DTO.User;
using AkaShi.Core.Logic.Abstractions;
using AkaShi.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AkaShi.WebAPI.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserDataGetter _userDataGetter;
    
    public UsersController(IUserService userService, IUserDataGetter userDataGetter)
    {
        _userService = userService;
        _userDataGetter = userDataGetter;
    }
    
    /// <summary>
    /// Get list of all users
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ICollection<UserDTO>>> Get()
    {
        return Ok(await _userService.GetUsers());
    }
    
    /// <summary>
    /// Get user information by ID
    /// </summary>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<UserDTO>> GetById(int id)
    {
        return Ok(await _userService.GetUserById(id));
    }
    
    /// <summary>
    /// Get current user based on token
    /// </summary>
    [HttpGet("fromToken")]
    public async Task<ActionResult<UserDTO>> GetUserFromToken()
    {
        return Ok(await _userService.GetUserById(_userDataGetter.CurrentUserId));
    }
    
    /// <summary>
    /// Update user
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] UserDTO user)
    {
        await _userService.UpdateUser(user);
        return NoContent();
    }

    /// <summary>
    /// Delete user by id
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _userService.DeleteUser(id);
        return NoContent();
    }
}