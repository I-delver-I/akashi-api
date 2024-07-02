using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts;
using AkaShi.Infrastructure.Context;
using AkaShi.Infrastructure.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace AkaShi.Infrastructure.Repositories;

public class LibraryVersionSupportedFrameworkRepository : BaseRepository, ILibraryVersionSupportedFrameworkRepository
{
    public LibraryVersionSupportedFrameworkRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<LibraryVersionSupportedFramework>> GetAllAsync()
    {
        return await Context.LibraryVersionSupportedFrameworks.ToListAsync();
    }

    public async Task<LibraryVersionSupportedFramework> GetByIdAsync(int id)
    {
        return await Context.LibraryVersionSupportedFrameworks
            .Include(sf => sf.Framework)
            .FirstOrDefaultAsync(sf => sf.Id == id);
    }

    public async Task AddAsync(LibraryVersionSupportedFramework entity)
    {
        await Context.LibraryVersionSupportedFrameworks.AddAsync(entity);
    }

    public void Delete(LibraryVersionSupportedFramework entity)
    {
        Context.Remove(entity);
    }

    public async Task DeleteByIdAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        Context.Remove(entity);
    }

    public void Update(LibraryVersionSupportedFramework entity)
    {
        throw new NotImplementedException();
    }
}