using AkaShi.Core.Common;
using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts;
using AkaShi.Core.DTO.Library;
using AkaShi.Core.DTO.LibraryVersion;
using AkaShi.Core.Exceptions;
using AkaShi.Core.Helpers;
using AkaShi.Core.Helpers.RepositoryParams;
using AkaShi.Core.Logic.Abstractions;
using AkaShi.Core.ServiceContracts;
using AkaShi.Core.Services.Abstract;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace AkaShi.Core.Services;

public class LibraryService : BaseService, ILibraryService
{
    private readonly ILibraryRepository _libraryRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IUserDataGetter _userDataGetter;
    
    private readonly IImageService _imageService;
    private readonly IFirebaseStorageService _firebaseStorageService;
    private readonly ILibraryVersionService _libraryVersionService;
    private readonly ILibraryVersionDependencyService _dependencyService;

    public LibraryService(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService,
        IFirebaseStorageService firebaseStorageService, IUserDataGetter userDataGetter,
        ILibraryVersionService libraryVersionService, ILibraryVersionDependencyService dependencyService) 
        : base(unitOfWork, mapper)
    {
        _libraryRepository = UnitOfWork.LibraryRepository;
        _imageRepository = UnitOfWork.ImageRepository;
        _userDataGetter = userDataGetter;
        
        _imageService = imageService;
        _firebaseStorageService = firebaseStorageService;
        _libraryVersionService = libraryVersionService;
        _dependencyService = dependencyService;
    }
    
    public async Task<LibraryDTO> GetLibraryByNameAsync(string name)
    {
        var library = await _libraryRepository.GetByNameAsync(name);
        if (library is null)
        {
            throw new NotFoundException(name);
        }

        return Mapper.Map<LibraryDTO>(library);
    }

    public async Task<PagedList<LibraryDTO>> GetLibrariesAsync(LibraryParams libraryParams)
    {
        var libraries = await _libraryRepository.GetAllAsync(libraryParams);

        return new PagedList<LibraryDTO>(Mapper.Map<IEnumerable<LibraryDTO>>(libraries.Items), 
            libraries.TotalCount, libraries.CurrentPage, libraries.PageSize);
    }

    public async Task<LibraryDTO> GetLibraryByIdAsync(int id)
    {
        var library = await _libraryRepository.GetByIdAsync(id);
        if (library is null)
        {
            throw new NotFoundException(nameof(Library), id);
        }

        return Mapper.Map<LibraryDTO>(library);
    }

    public async Task<LibraryDTO> CreateLibraryAsync(NewLibraryDTO newLibraryDto)
    {
        if (newLibraryDto is null)
        {
            throw new ArgumentException("Library data cannot be null.");
        }
        
        var libraryEntity = Mapper.Map<Library>(newLibraryDto);
        libraryEntity.LastUpdateTime = DateTime.UtcNow;
        libraryEntity.UserId = _userDataGetter.CurrentUserId;
        libraryEntity.LogoId = ApplicationConstants.DefaultLogoId;
        
        try
        {
            await _libraryRepository.AddAsync(libraryEntity);
            await UnitOfWork.SaveAsync();

            var newLibraryVersionDto = new NewLibraryVersionDTO
            {
                Name = newLibraryDto.InitialVersionName,
                LibraryVersionArchive = newLibraryDto.InitialVersionArchive,
                LibraryId = libraryEntity.Id
            };
            await _libraryVersionService.CreateLibraryVersionAsync(newLibraryVersionDto);
            
            libraryEntity = await _libraryRepository.GetByIdAsync(libraryEntity.Id);
            return Mapper.Map<LibraryDTO>(libraryEntity);
        }
        catch (DbUpdateException ex)
        {
            _libraryRepository.Delete(libraryEntity);
            Log.Error(ex, "Error occurred while creating a new library: {LibraryName}", 
                newLibraryDto.Name);
            throw new LibraryCreationException("An error occurred while creating the library.", ex);
        }
        catch (Exception ex)
        {
            _libraryRepository.Delete(libraryEntity);
            Log.Error(ex, "Unexpected error occurred while creating a new library");
            throw;
        }
    }

    public async Task UpdateLibraryAsync(UpdateLibraryDTO updateLibraryDto)
    {
        var libraryEntity = await _libraryRepository.GetByIdAsync(updateLibraryDto.Id);
        if (libraryEntity is null)
        {
            throw new NotFoundException(nameof(Library), updateLibraryDto.Id);
        }
        
        libraryEntity.LastUpdateTime = DateTime.Now;
        libraryEntity.ShortDescription = updateLibraryDto.ShortDescription;

        if (!ValidateLibraryTags(updateLibraryDto.Tags))
        {
            throw new ArgumentException("Invalid library tags provided.");
        }
        
        libraryEntity.Tags = updateLibraryDto.Tags;
        libraryEntity.ProjectWebsiteURL = updateLibraryDto.ProjectWebsiteURL;

        await HandleLibraryLogoUpdate(libraryEntity, updateLibraryDto);
        await UnitOfWork.SaveAsync();
    }
    
    private async Task HandleLibraryLogoUpdate(Library libraryEntity, UpdateLibraryDTO updateLibraryDto)
    {
        var libraryOwnerUsername = libraryEntity.User.Username;
        var libraryStoragePath = $"{libraryOwnerUsername}/{libraryEntity.Name}";

        if (updateLibraryDto.Logo is not null)
        {
            await ProcessNewLogo(libraryEntity, updateLibraryDto, libraryStoragePath);
        }
        else if (libraryEntity.Logo.Id != ApplicationConstants.DefaultLogoId)
        {
            await _imageService.DeleteImage(libraryEntity.Logo.Id, libraryStoragePath);
            libraryEntity.Logo = await _imageRepository.GetByIdAsync(ApplicationConstants.DefaultLogoId);
        }
    }
    
    private async Task ProcessNewLogo(Library libraryEntity, UpdateLibraryDTO updateLibraryDto, 
        string libraryStoragePath)
    {
        if (libraryEntity.Logo.Id == ApplicationConstants.DefaultLogoId)
        {
            var createdImage = await _imageService.CreateImage(updateLibraryDto.Logo, libraryStoragePath);
            libraryEntity.Logo = await _imageRepository.GetByIdAsync(createdImage.Id);
        }
        else
        {
            await _imageService.UpdateImage(updateLibraryDto.Logo, libraryStoragePath, libraryEntity.Logo);
        }
    }

    private static bool ValidateLibraryTags(string tags)
    {
        if (string.IsNullOrEmpty(tags))
        {
            return true;
        }
        
        var separatedTags = tags.Split(' ');
        
        foreach (var tag in separatedTags)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                return false;
            }

            if (!tag.All(char.IsLetterOrDigit))
            {
                return false;
            }
        }

        return true;
    }
    
    public async Task DeleteLibraryAsync(int id)
    {
        await using var transaction = await UnitOfWork.BeginTransactionAsync();
        
        try
        {
            var libraryEntity = await _libraryRepository.GetByIdAsync(id);
            if (libraryEntity is null)
            {
                throw new NotFoundException(nameof(Library), id);
            }

            foreach (var libraryVersion in libraryEntity.LibraryVersions)
            {
                await _libraryVersionService.DeleteLibraryVersionAsync(libraryVersion.Id);
            }

            foreach (var dependency in libraryEntity.LibraryVersionDependencies)
            {
                await _dependencyService.DeleteLibraryVersionDependencyAsync(dependency.Id);
            }

            var libraryOwnerUsername = libraryEntity.User.Username;
            var libraryStoragePath = $"{libraryOwnerUsername}/{libraryEntity.Name}";

            if (libraryEntity.LogoId != null)
            {
                await _imageService.DeleteImage((int)libraryEntity.LogoId, libraryStoragePath);
            }

            await _libraryRepository.DeleteByIdAsync(id);
            await UnitOfWork.SaveAsync();

            await _firebaseStorageService.RemoveAsync($"{libraryStoragePath}/");
            
            await UnitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await UnitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}