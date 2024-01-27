using AkaShi.Core.Domain.Entities;
using AkaShi.Core.DTO.Image;
using Microsoft.AspNetCore.Http;

namespace AkaShi.Core.ServiceContracts;

public interface IImageService
{
    Task<ICollection<ImageDTO>> GetImages();
    Task<ImageDTO> GetImageById(int id);
    Task<ImageDTO> CreateImage(IFormFile imageFile, string libraryStoragePath);
    Task<ImageDTO> UpdateImage(IFormFile newImageFile, string libraryStoragePath, Image entityToUpdate);
    Task DeleteImage(int id, string libraryStoragePath);
}