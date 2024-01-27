using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts;
using AkaShi.Infrastructure.Context;
using AkaShi.Infrastructure.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace AkaShi.Infrastructure.Repositories;

public class ImageRepository : BaseRepository, IImageRepository
{
    public ImageRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<IEnumerable<Image>> GetAllAsync()
    {
        return await Context.Images.ToArrayAsync();
    }

    public async Task<Image> GetByIdAsync(int id)
    {
        return await Context.Images
            .Include(i => i.FileExtension)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task AddAsync(Image entity)
    {
        await Context.Images.AddAsync(entity);
    }

    public void Delete(Image entity)
    {
        Context.Images.Remove(entity);
    }

    public async Task DeleteByIdAsync(int id)
    { 
        var image = await GetByIdAsync(id);
        Context.Images.Remove(image);
    }

    public void Update(Image entity)
    {
        Context.Images.Update(entity);
    }
}