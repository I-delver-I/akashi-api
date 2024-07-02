using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts;
using AkaShi.Core.DTO.LibraryVersionSupportedFramework;
using AkaShi.Core.Exceptions;
using AkaShi.Core.ServiceContracts;
using AkaShi.Core.Services.Abstract;
using AutoMapper;

namespace AkaShi.Core.Services;

public class LibraryVersionSupportedFrameworkService : BaseService, ILibraryVersionSupportedFrameworkService
{
    private readonly ILibraryVersionSupportedFrameworkRepository _libraryVersionSupportedFrameworkRepository;
    
    public LibraryVersionSupportedFrameworkService(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork, mapper)
    {
        _libraryVersionSupportedFrameworkRepository = UnitOfWork.LibraryVersionSupportedFrameworkRepository;
    }

    public async Task<ICollection<LibraryVersionSupportedFrameworkDTO>> 
        GetLibraryVersionSupportedFrameworksAsync()
    {
        var frameworks = 
            await _libraryVersionSupportedFrameworkRepository.GetAllAsync();
        return Mapper.Map<ICollection<LibraryVersionSupportedFrameworkDTO>>(frameworks);
    }

    public async Task<LibraryVersionSupportedFrameworkDTO> GetLibraryVersionSupportedFrameworkByIdAsync(int id)
    { 
        var framework = await _libraryVersionSupportedFrameworkRepository.GetByIdAsync(id);
        
        if (framework is null)
        {
            throw new NotFoundException($"LibraryVersionSupportedFramework with ID {id} not found.");
        }
        
        return Mapper.Map<LibraryVersionSupportedFrameworkDTO>(framework);
    }

    public async Task<LibraryVersionSupportedFrameworkDTO> CreateLibraryVersionSupportedFrameworkAsync(
        NewLibraryVersionSupportedFrameworkDTO dto)
    {
        if (dto == null)
        {
            throw new ArgumentException("Supported framework data cannot be null.");
        }
        
        var entity = Mapper.Map<LibraryVersionSupportedFramework>(dto);

        try
        {
            await _libraryVersionSupportedFrameworkRepository.AddAsync(entity);
            await UnitOfWork.SaveAsync();
            return Mapper.Map<LibraryVersionSupportedFrameworkDTO>(entity);
        }
        catch (Exception ex)
        {
            throw new LibraryVersionSupportedFrameworkCreationException
                ("Error occurred while creating the supported framework.", ex);
        }
    }

    public async Task DeleteLibraryVersionSupportedFrameworkAsync(int id)
    {
        var libraryVersionSupportedFrameworkEntity = 
            await _libraryVersionSupportedFrameworkRepository.GetByIdAsync(id);
        if (libraryVersionSupportedFrameworkEntity is null)
        {
            throw new NotFoundException(nameof(LibraryVersionSupportedFramework), id);
        }

        _libraryVersionSupportedFrameworkRepository.Delete(libraryVersionSupportedFrameworkEntity);
        await UnitOfWork.SaveAsync();
    }
}