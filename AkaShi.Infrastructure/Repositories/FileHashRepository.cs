using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts;
using AkaShi.Infrastructure.Context;
using AkaShi.Infrastructure.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace AkaShi.Infrastructure.Repositories;

public class FileHashRepository : BaseRepository, IFileHashRepository
{
    public FileHashRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<LibraryArchiveHash>> GetAllAsync()
    {
        return await Context.LibraryArchiveHashes.ToListAsync();
    }

    public async Task<LibraryArchiveHash> GetByIdAsync(int id)
    {
        return await Context.LibraryArchiveHashes.FindAsync(id);
    }

    public async Task AddAsync(LibraryArchiveHash entity)
    {
        await Context.LibraryArchiveHashes.AddAsync(entity);
    }

    public void Delete(LibraryArchiveHash entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(LibraryArchiveHash entity)
    { 
        throw new NotImplementedException();
    }

    public async Task<LibraryArchiveHash> GetByHashAsync(string fileHash)
    {
        return await Context.LibraryArchiveHashes.FirstOrDefaultAsync(f => f.Hash == fileHash);
    }
}