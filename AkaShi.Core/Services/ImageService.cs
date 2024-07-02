using AkaShi.Core.Common;
using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts;
using AkaShi.Core.DTO.Image;
using AkaShi.Core.Exceptions;
using AkaShi.Core.ServiceContracts;
using AkaShi.Core.Services.Abstract;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace AkaShi.Core.Services;

public class ImageService : BaseService, IImageService
{
    private readonly IImageRepository _imageRepository;
    private readonly IFileExtensionRepository _fileExtensionRepository;
    
    private readonly IFirebaseStorageService _firebaseStorageService;
    
    private readonly HashSet<string> _allowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".png", ".jpg", ".jpeg", ".webp"
    };
    
    public ImageService(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseStorageService firebaseStorageService) 
        : base(unitOfWork, mapper)
    {
        _imageRepository = UnitOfWork.ImageRepository;
        _fileExtensionRepository = UnitOfWork.FileExtensionRepository;
        _firebaseStorageService = firebaseStorageService;
    }

    public async Task<ICollection<ImageDTO>> GetImages()
    {
        var images = await _imageRepository.GetAllAsync();
        return Mapper.Map<ICollection<ImageDTO>>(images);
    }

    public async Task<ImageDTO> GetImageById(int id)
    {
        var image = await _imageRepository.GetByIdAsync(id);
        if (image is null)
        {
            throw new NotFoundException(nameof(Image), id);
        }
        
        return Mapper.Map<ImageDTO>(image);
    }

    public async Task<ImageDTO> CreateImage(IFormFile imageFile, string libraryStoragePath)
    {
        var imageExtension = Path.GetExtension(imageFile.FileName);
        
        if (!_allowedExtensions.Contains(imageExtension))
        {
            throw new ArgumentException("Unsupported image file type.");
        }

        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var url = await CreateImageInStorage(imageFile, libraryStoragePath, timestamp, imageExtension);
        
        var extensionEntity = await _fileExtensionRepository.GetFileExtensionByName(imageExtension);
        var imageToCreate = new Image { URL = url, FileExtensionId = extensionEntity.Id, Timestamp = timestamp };
        await _imageRepository.AddAsync(imageToCreate);
        
        await UnitOfWork.SaveAsync();
        return Mapper.Map<ImageDTO>(imageToCreate);
    }
    
    public async Task<ImageDTO> UpdateImage(IFormFile newImageFile, string libraryStoragePath, Image entityToUpdate)
    {
        await RemoveImageFromStorage(libraryStoragePath, entityToUpdate.Timestamp, entityToUpdate.FileExtension.Name);
        
        var newImageExtension = Path.GetExtension(newImageFile.FileName);
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var url = await CreateImageInStorage(newImageFile, libraryStoragePath, timestamp, newImageExtension);
        
        var extensionEntity = await _fileExtensionRepository.GetFileExtensionByName(newImageExtension);
        entityToUpdate.FileExtensionId = extensionEntity.Id;
        entityToUpdate.URL = url;
        entityToUpdate.Timestamp = timestamp;
        
        _imageRepository.Update(entityToUpdate);
        await UnitOfWork.SaveAsync();
        return Mapper.Map<ImageDTO>(entityToUpdate);
    }

    private async Task<string> CreateImageInStorage(IFormFile imageFile, string libraryStoragePath, 
        long timestamp, string newImageExtension)
    {
        var storageImagePath = 
            $"{libraryStoragePath}/{ApplicationConstants.DefaultLogoName}_{timestamp}{newImageExtension}";
        await using var imageStream = imageFile.OpenReadStream();
        await _firebaseStorageService.UploadAsync(imageStream, storageImagePath, imageFile.ContentType);
        return await _firebaseStorageService.MakeFilePublicAndGetUrlAsync(storageImagePath);
    }
    
    private async Task RemoveImageFromStorage(string libraryStoragePath, long timestamp, string imageExtension)
    {
        var storageImagePath = 
            $"{libraryStoragePath}/{ApplicationConstants.DefaultLogoName}_{timestamp}{imageExtension}";
        await _firebaseStorageService.RemoveAsync(storageImagePath);
    }
    
    public async Task DeleteImage(int id, string libraryStoragePath)
    {
        if (id == ApplicationConstants.DefaultLogoId)
        {
            return;
        }
        
        var entity = await GetImageById(id);
        
        await _imageRepository.DeleteByIdAsync(id);
        await UnitOfWork.SaveAsync();
        
        await RemoveImageFromStorage(libraryStoragePath, entity.Timestamp, entity.FileExtensionName);
    }
}