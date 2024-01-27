using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts;
using AkaShi.Infrastructure.Context;
using AkaShi.Infrastructure.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace AkaShi.Infrastructure.Repositories;

public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await Context.Users.ToListAsync();
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await Context.Users.FindAsync(id);
    }

    public async Task AddAsync(User entity)
    {
        await Context.Users.AddAsync(entity);
    }

    public void Delete(User entity)
    {
        Context.Users.Remove(entity);
    }

    public async Task DeleteByIdAsync(int id)
    {
        var user = await GetByIdAsync(id);

        if (user != null)
        {
            Context.Users.Remove(user);
        }
    }

    public void Update(User entity)
    {
        Context.Users.Update(entity);
    }
}