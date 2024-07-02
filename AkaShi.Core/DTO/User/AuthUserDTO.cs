using AkaShi.Core.DTO.Auth;

namespace AkaShi.Core.DTO.User;

public sealed class AuthUserDTO
{
    public UserDTO User { get; set; }
    public AccessTokenDTO Token { get; set; }
}