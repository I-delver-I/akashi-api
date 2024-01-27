using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts;
using AkaShi.Core.Helpers;
using AkaShi.Core.Helpers.RepositoryParams;
using AkaShi.Infrastructure.Context;
using AkaShi.Infrastructure.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace AkaShi.Infrastructure.Repositories;

public class LibraryRepository : BaseRepository, ILibraryRepository
{
    public LibraryRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<IEnumerable<Library>> GetAllAsync()
    {
        return await Context.Libraries
            .Include(lib => lib.Logo)
            .Include(lib => lib.User)
            .ToListAsync();
    }

    public async Task<Library> GetByIdAsync(int id)
    {
        return await Context.Libraries
            .Include(lib => lib.Logo)
                .ThenInclude(img => img.FileExtension)
            .Include(lib => lib.User)
            .Include(lib => lib.LibraryVersionDependencies)
            .Include(lib => lib.LibraryVersions)
            .FirstOrDefaultAsync(lib => lib.Id == id);
    }
    
    public async Task<Library> GetByNameAsync(string name)
    {
        return await Context.Libraries.FirstOrDefaultAsync(l => l.Name == name);
    }

    public async Task<IEnumerable<Library>> GetTopDownloadedLibrariesAsync(int count)
    {
        return await Context.Libraries
            .OrderByDescending(lib => lib.DownloadsCount)
            .Take(count)
            .ToListAsync();
    }

    public async Task<PagedList<Library>> GetAllAsync(LibraryParams libraryParams)
    {
        var query = Context.Libraries
                .Include(lib => lib.Logo)
                .Include(lib => lib.User)
                /*.Include(lib => lib.LibraryVersions)
                    .ThenInclude(lv => lv.LibraryVersionSupportedFrameworks)
                        .ThenInclude(sf => sf.Framework)*/
                .AsQueryable();
        
        if (libraryParams.SearchTerm is not null)
        {
            query = query.Where(f => f.Name.ToLower().Contains(libraryParams.SearchTerm.ToLower()));
        }
        
        /*var filter = libraryParams.FilterParams.FrameworkProductNameFilter;

        if (filter.DotNet)
        {
            query = query.Where(l => l.LibraryVersions.Any(lv => lv.LibraryVersionSupportedFrameworks
                .Any(f => f.Framework.ProductName == ".NET")));
        }

        if (filter.DotNetCore)
        {
            query = query.Where(l => l.LibraryVersions.Any(lv => lv.LibraryVersionSupportedFrameworks
                .Any(f => f.Framework.ProductName == ".NET Core")));
        }
        
        if (filter.DotNetFramework)
        {
            query = query.Where(l => l.LibraryVersions.Any(lv => lv.LibraryVersionSupportedFrameworks
                .Any(f => f.Framework.ProductName == ".NET Framework")));
        }
        
        if (filter.DotNetStandard)
        {
            query = query.Where(l => l.LibraryVersions.Any(lv => lv.LibraryVersionSupportedFrameworks
                .Any(f => f.Framework.ProductName == ".NET Standard")));
        }*/

        if (libraryParams.OrderByParams is { OrderBy: not null })
        {
            query = libraryParams.OrderByParams.OrderBy switch
            {
                OrderBy.Downloads => query.OrderByDescending(l => l.DownloadsCount),
                OrderBy.LastUpdated => query.OrderByDescending(l => l.LastUpdateTime),
                _ => query
            };
        }

        return await PagedList<Library>.CreateAsync(query, libraryParams.PageNumber, libraryParams.PageSize);
    }

    public async Task AddAsync(Library entity)
    {
        await Context.Libraries.AddAsync(entity);
    }

    public void Delete(Library entity)
    {
        Context.Libraries.Remove(entity);
    }

    public async Task DeleteByIdAsync(int id)
    {
        var library = await GetByIdAsync(id); 
        Context.Libraries.Remove(library);
    }

    public void Update(Library entity)
    {
        Context.Libraries.Update(entity);
    }
}