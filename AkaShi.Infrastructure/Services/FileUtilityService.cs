using System.Security.Cryptography;
using AkaShi.Core.Domain.RepositoryContracts;
using AkaShi.Core.ServiceContracts;
using AkaShi.Core.Services.Abstract;
using AutoMapper;

namespace AkaShi.Infrastructure.Services;

public class FileUtilityService : BaseService, IFileUtilityService
{
    private readonly IFileHashRepository _fileHashRepository;
    
    public FileUtilityService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
        _fileHashRepository = UnitOfWork.FileHashRepository;
    }
    
    public async Task<string> CalculateFileHashAsync(Stream fileStream)
    {
        using var sha256 = SHA256.Create();
        var hash = await sha256.ComputeHashAsync(fileStream);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }

    public async Task<bool> HashExistsAsync(string fileHash)
    {
        var fileHashEntity = await _fileHashRepository.GetByHashAsync(fileHash);
        return fileHashEntity is not null;
    }
}