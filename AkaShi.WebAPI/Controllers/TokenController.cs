using AkaShi.Core.DTO.Auth;
using AkaShi.Core.ServiceContracts;
using AkaShi.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AkaShi.WebAPI.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public TokenController(IAuthService authService)
    {
        _authService = authService;
    }
    
    /// <summary>
    /// Refreshes user token and creates new access token
    /// </summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<AccessTokenDTO>> Refresh([FromBody] RefreshTokenDTO dto)
    {
        return Ok(await _authService.RefreshToken(dto));
    }

    /// <summary>
    /// Revokes refresh tokens
    /// </summary>
    [HttpPost("revoke")]
    public async Task<IActionResult> RevokeRefreshToken([FromBody] RevokeRefreshTokenDTO dto)
    {
        await _authService.RevokeRefreshToken(dto.RefreshToken);
        return Ok();
    }
}