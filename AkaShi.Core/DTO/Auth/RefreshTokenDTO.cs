using System.Text.Json.Serialization;

namespace AkaShi.Core.DTO.Auth;

public sealed class RefreshTokenDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

    [JsonIgnore]
    public string SigningKey { get; private set; } = Environment.GetEnvironmentVariable("SecretJWTKey");
}