using AkaShi.Core.DTO.Auth;
using AkaShi.Core.DTO.User;

namespace AkaShi.Core.ServiceContracts;

public interface IAuthService
{
    Task<AuthUserDTO> Authorize(UserLoginDTO userDto);
    Task<AccessTokenDTO> GenerateAccessToken(int userId, string userName, string email);
    Task<AccessTokenDTO> RefreshToken(RefreshTokenDTO dto);
    Task RevokeRefreshToken(string refreshToken);
}