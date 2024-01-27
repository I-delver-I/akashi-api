using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts;
using AkaShi.Infrastructure.Context;
using AkaShi.Infrastructure.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace AkaShi.Infrastructure.Repositories;

public class LibraryVersionDependencyRepository : BaseRepository, ILibraryVersionDependencyRepository
{
    public LibraryVersionDependencyRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<LibraryVersionDependency>> GetAllAsync()
    {
        return await Context.LibraryVersionDependencies.ToListAsync();
    }

    public async Task<LibraryVersionDependency> GetByIdAsync(int id)
    {
        return await Context.LibraryVersionDependencies
            .Include(d => d.Framework)
            .Include(d => d.DependencyLibrary)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task AddAsync(LibraryVersionDependency entity)
    {
        await Context.LibraryVersionDependencies.AddAsync(entity);
    }

    public void Delete(LibraryVersionDependency entity)
    {
        Context.Remove(entity);
    }

    public async Task DeleteByIdAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        Context.Remove(entity);
    }

    public void Update(LibraryVersionDependency entity)
    {
        Context.LibraryVersionDependencies.Update(entity);
    }
}