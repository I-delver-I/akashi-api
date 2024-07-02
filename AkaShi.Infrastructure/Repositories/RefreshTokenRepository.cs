using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts;
using AkaShi.Infrastructure.Context;
using AkaShi.Infrastructure.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace AkaShi.Infrastructure.Repositories;

public class RefreshTokenRepository : BaseRepository, IRefreshTokenRepository
{
    public RefreshTokenRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<RefreshToken>> GetAllAsync()
    {
        return await Context.RefreshTokens.ToListAsync();
    }

    public async Task<RefreshToken> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(RefreshToken entity)
    {
        await Context.RefreshTokens.AddAsync(entity);
    }

    public void Delete(RefreshToken entity)
    {
        Context.Remove(entity);
    }

    public async Task DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(RefreshToken entity)
    {
        throw new NotImplementedException();
    }

    public Task<RefreshToken> GetByTokenAndUserIdAsync(string token, int userId)
    {
        return Context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token && t.UserId == userId);
    }
}