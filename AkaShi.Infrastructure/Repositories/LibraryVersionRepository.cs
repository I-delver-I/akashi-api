using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts;
using AkaShi.Core.Helpers;
using AkaShi.Core.Helpers.RepositoryParams;
using AkaShi.Infrastructure.Context;
using AkaShi.Infrastructure.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace AkaShi.Infrastructure.Repositories;

public class LibraryVersionRepository : BaseRepository, ILibraryVersionRepository
{
    public LibraryVersionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<LibraryVersion>> GetByLibraryIdAsync(int id)
    {
        return await Context.LibraryVersions
            .Where(lv => lv.LibraryId == id)
            .Include(lv => lv.Library)
            .Include(lv => lv.FileExtension)
            .ToListAsync();
    }

    public async Task<PagedList<LibraryVersion>> GetAllAsync(LibraryVersionParams libraryVersionParams)
    {
        var query = Context.LibraryVersions
            .Include(lv => lv.Library)
            .Include(lv => lv.FileExtension)
            .AsQueryable();
        
        return await PagedList<LibraryVersion>.CreateAsync
            (query, libraryVersionParams.PageNumber, libraryVersionParams.PageSize);
    }

    public async Task<IEnumerable<LibraryVersion>> GetAllAsync()
    {
        return await Context.LibraryVersions
            .Include(lv => lv.Library)
            .Include(lv => lv.FileExtension)
            .ToListAsync();
    }

    public async Task<LibraryVersion> GetByIdAsync(int id)
    {
        return await Context.LibraryVersions
            .Include(lv => lv.Library)
                .ThenInclude(l => l.User)
            .Include(lv => lv.FileExtension)
            
            .Include(lv => lv.LibraryVersionDependencies)
                .ThenInclude(d => d.Framework)
            .Include(lv => lv.LibraryVersionDependencies)
                .ThenInclude(d => d.DependencyLibrary)
            
            .Include(lv => lv.LibraryVersionSupportedFrameworks)
                .ThenInclude(sf => sf.Framework)
            
            .FirstOrDefaultAsync(lv => lv.Id == id);
    }

    public async Task AddAsync(LibraryVersion entity)
    {
        await Context.LibraryVersions.AddAsync(entity);
    }

    public void Delete(LibraryVersion entity)
    {
        Context.LibraryVersions.Remove(entity);
    }

    public async Task DeleteByIdAsync(int id)
    {
        var libraryVersion = await GetByIdAsync(id);
        Context.LibraryVersions.Remove(libraryVersion);
    }

    public void Update(LibraryVersion entity)
    {
        Context.LibraryVersions.Update(entity);
    }
}