using AkaShi.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AkaShi.WebAPI.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static int GetUserIdFromToken(this ControllerBase controller)
        {
            var claimsUserId = controller.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;

            if (string.IsNullOrEmpty(claimsUserId))
            {
                throw new InvalidTokenException("access");
            }

            return int.Parse(claimsUserId);
        }
        
        public static string GetUsernameFromToken(this ControllerBase controller)
        {
            var claimsUsername = controller.User.Claims.FirstOrDefault(x => x.Type == "userName")?.Value;

            if (string.IsNullOrEmpty(claimsUsername))
            {
                throw new InvalidTokenException("access");
            }

            return claimsUsername;
        }
    }
}
