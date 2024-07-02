using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts;
using AkaShi.Infrastructure.Context;
using AkaShi.Infrastructure.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace AkaShi.Infrastructure.Repositories;

public class FileExtensionRepository : BaseRepository, IFileExtensionRepository
{
    public FileExtensionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<FileExtension>> GetAllAsync()
    {
        return await Context.FileExtensions.ToListAsync();
    }

    public async Task<FileExtension> GetByIdAsync(int id)
    {
        return await Context.FileExtensions.FindAsync(id);
    }

    public async Task AddAsync(FileExtension entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(FileExtension entity)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(FileExtension entity)
    {
        throw new NotImplementedException();
    }

    public Task<FileExtension> GetFileExtensionByName(string name)
    {
        return Context.FileExtensions.FirstOrDefaultAsync(fe => fe.Name == name);
    }
}