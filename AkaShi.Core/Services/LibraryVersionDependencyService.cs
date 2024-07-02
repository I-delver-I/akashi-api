using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts;
using AkaShi.Core.DTO.LibraryVersionDependency;
using AkaShi.Core.Exceptions;
using AkaShi.Core.ServiceContracts;
using AkaShi.Core.Services.Abstract;
using AutoMapper;
using Serilog;

namespace AkaShi.Core.Services;

public class LibraryVersionDependencyService : BaseService, ILibraryVersionDependencyService
{
    private readonly ILibraryVersionDependencyRepository _libraryVersionDependencyRepository;
    
    public LibraryVersionDependencyService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
        _libraryVersionDependencyRepository = UnitOfWork.LibraryVersionDependencyRepository;
    }
    
    public async Task<ICollection<LibraryVersionDependencyDTO>> GetLibraryVersionDependenciesAsync()
    {
        var dependencies = await _libraryVersionDependencyRepository.GetAllAsync();
        return Mapper.Map<ICollection<LibraryVersionDependencyDTO>>(dependencies);
    }

    public async Task<LibraryVersionDependencyDTO> GetLibraryVersionDependencyByIdAsync(int id)
    {
        var dependency = await _libraryVersionDependencyRepository.GetByIdAsync(id);
        if (dependency is null)
        {
            throw new NotFoundException($"LibraryVersionDependency with ID {id} not found.");
        }
        
        return Mapper.Map<LibraryVersionDependencyDTO>(dependency);
    }

    public async Task<LibraryVersionDependencyDTO> CreateLibraryVersionDependencyAsync
        (NewLibraryVersionDependencyDTO newLibraryVersionDependencyDto)
    {
        var entity = Mapper.Map<LibraryVersionDependency>(newLibraryVersionDependencyDto);

        try
        {
            await _libraryVersionDependencyRepository.AddAsync(entity);
            await UnitOfWork.SaveAsync();
            Log.Information("Created LibraryVersionDependency with ID {DependencyId}", entity.Id);
            return Mapper.Map<LibraryVersionDependencyDTO>(entity);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while creating LibraryVersionDependency");
            throw;
        }
    }

    public async Task DeleteLibraryVersionDependencyAsync(int id)
    {
        var entity = await _libraryVersionDependencyRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new NotFoundException($"LibraryVersionDependency with ID {id} not found.");
        }

        try
        {
            _libraryVersionDependencyRepository.Delete(entity);
            await UnitOfWork.SaveAsync();
            Log.Information("Deleted LibraryVersionDependency with ID {DependencyId}", id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while deleting LibraryVersionDependency " +
                          "with ID {DependencyId}", id);
            throw;
        }
    }
}