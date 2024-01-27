using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts.Abstract;

namespace AkaShi.Core.Domain.RepositoryContracts;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<RefreshToken> GetByTokenAndUserIdAsync(string token, int userId);
}