using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts;
using AkaShi.Infrastructure.Context;
using AkaShi.Infrastructure.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace AkaShi.Infrastructure.Repositories;

public class FrameworkRepository : BaseRepository, IFrameworkRepository
{
    public FrameworkRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Framework>> GetAllAsync()
    {
        return await Context.Frameworks.ToListAsync();
    }

    public async Task<Framework> GetByIdAsync(int id)
    {
        return await Context.Frameworks.FindAsync(id);
    }

    public async Task AddAsync(Framework entity)
    {
        await Context.Frameworks.AddAsync(entity);
    }

    public void Delete(Framework entity)
    {
        Context.Remove(entity);
    }

    public async Task DeleteByIdAsync(int id)
    {
        var framework = await GetByIdAsync(id);
        Context.Remove(framework);
    }

    public void Update(Framework entity)
    {
        Context.Frameworks.Update(entity);
    }

    public async Task<Framework> GetByVersionNameAsync(string name)
    {
        return await Context.Frameworks.FirstOrDefaultAsync(f => f.VersionName == name);
    }
}