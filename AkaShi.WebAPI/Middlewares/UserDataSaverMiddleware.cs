using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AkaShi.Core.Logic.Abstractions;

namespace AkaShi.WebAPI.Middlewares;

public class UserDataSaverMiddleware
{
    private readonly RequestDelegate _next;

    public UserDataSaverMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task InvokeAsync(HttpContext context, IUserDataSetter userDataSetter)
    {
        var claimsUserId = context.User.Claims
            .FirstOrDefault(x => x.Type == "id")?.Value;
        var claimsUserName = context.User.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (claimsUserId != null && int.TryParse(claimsUserId, out var userId))
        {
            userDataSetter.SetUserId(userId);
        }
        
        if (!string.IsNullOrEmpty(claimsUserName))
        {
            userDataSetter.SetUserName(claimsUserName);
        }

        return _next.Invoke(context);
    }
}