using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts;
using AkaShi.Core.DTO.LibraryVersion;
using AkaShi.Core.DTO.LibraryVersionDependency;
using AkaShi.Core.DTO.LibraryVersionSupportedFramework;
using AkaShi.Core.Exceptions;
using AkaShi.Core.Helpers;
using AkaShi.Core.Helpers.RepositoryParams;
using AkaShi.Core.Logic.Abstractions;
using AkaShi.Core.ServiceContracts;
using AkaShi.Core.Services.Abstract;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Serilog;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Writers;

namespace AkaShi.Core.Services;

public class LibraryVersionService : BaseService, ILibraryVersionService
{
    private readonly ILibraryVersionRepository _libraryVersionRepository;
    private readonly IFileExtensionRepository _fileExtensionRepository;
    private readonly IFrameworkRepository _frameworkRepository;
    private readonly IFileHashRepository _fileHashRepository;
    private readonly ILibraryRepository _libraryRepository;
    private readonly ILibraryVersionDependencyRepository _libraryVersionDependencyRepository;
    
    private readonly IFirebaseStorageService _firebaseStorageService;
    private readonly ILibraryArchiveValidationService _libraryArchiveValidationService;
    private readonly IUserDataGetter _userDataGetter;
    private readonly ILibraryArchiveExtractorService _libraryArchiveExtractorService;
    private readonly ILibraryVersionDependencyService _libraryVersionDependencyService;
    private readonly ILibraryVersionSupportedFrameworkService _libraryVersionSupportedFrameworkService;
    private readonly IFileUtilityService _fileUtilityService;

    public LibraryVersionService(IUnitOfWork unitOfWork, IMapper mapper, IUserDataGetter userDataGetter, 
        IFirebaseStorageService firebaseStorageService, 
        ILibraryArchiveValidationService libraryArchiveValidationService,
        ILibraryArchiveExtractorService libraryArchiveExtractorService,
        ILibraryVersionDependencyService libraryVersionDependencyService,
        ILibraryVersionSupportedFrameworkService libraryVersionSupportedFrameworkService,
        IFileUtilityService fileUtilityService) : base(unitOfWork, mapper)
    {
        _libraryVersionRepository = UnitOfWork.LibraryVersionRepository;
        _fileExtensionRepository = UnitOfWork.FileExtensionRepository;
        _frameworkRepository = UnitOfWork.FrameworkRepository;
        _fileHashRepository = UnitOfWork.FileHashRepository;
        _libraryRepository = UnitOfWork.LibraryRepository;
        _libraryVersionDependencyRepository = UnitOfWork.LibraryVersionDependencyRepository;
        
        _firebaseStorageService = firebaseStorageService;
        _libraryArchiveValidationService = libraryArchiveValidationService;
        _userDataGetter = userDataGetter;
        _libraryArchiveExtractorService = libraryArchiveExtractorService;
        _libraryVersionDependencyService = libraryVersionDependencyService;
        _libraryVersionSupportedFrameworkService = libraryVersionSupportedFrameworkService;
        _fileUtilityService = fileUtilityService;
    }
    
    public async Task<ICollection<LibraryVersionDependencyDTO>> GetLibraryVersionDependenciesAsync(int libraryVersionId)
    {
        var versionDependencies = await _libraryVersionDependencyRepository.GetByLibraryVersionId(libraryVersionId);
        if (versionDependencies is null)
        {
            throw new NotFoundException(nameof(LibraryVersion), libraryVersionId);
        }

        return Mapper.Map<ICollection<LibraryVersionDependencyDTO>>(versionDependencies);
    }

    public async Task<IEnumerable<LibraryVersionDTO>> GetLibraryVersionsByLibraryIdAsync(int id)
    {
        var libraryVersions = await _libraryVersionRepository
            .GetByLibraryIdAsync(id);
        
        return Mapper.Map<IEnumerable<LibraryVersionDTO>>(libraryVersions);
    }

    public async Task<DownloadLibraryVersionDTO> DownloadLibraryVersionAsync(int id, string archiveFormat)
    {
        var libraryVersion = await _libraryVersionRepository.GetByIdAsync(id);
        if (libraryVersion == null)
        {
            throw new NotFoundException($"Library version with ID {id} not found.");
        }

        var libraryOwnerUserName = libraryVersion.Library.User.Username;
        var archiveFullName = $"{libraryVersion.Library.Name}_{libraryVersion.Name}{libraryVersion.FileExtension.Name}";
        var archiveStoragePath = $"{libraryOwnerUserName}/{libraryVersion.Library.Name}/{archiveFullName}";
        
        var downloadedLibraryVersionDTO = await _firebaseStorageService.DownloadAsync(archiveStoragePath);
        if (downloadedLibraryVersionDTO.FileContent is null)
        {
            throw new NotFoundException("Library file not found.");
        }
        
        var convertedFile = await ConvertFileFormat(downloadedLibraryVersionDTO, archiveFormat);
        if (convertedFile is null)
        {
            throw new Exception("Error converting file.");
        }

        await IncrementLibraryVersionDownloadsAsync(id);
        
        return new DownloadLibraryVersionDTO
        {
            FileContent = convertedFile.FileContent,
            ContentType = convertedFile.ContentType,
            FileName = convertedFile.FileName
        };
    }
    
    /*private async Task<DownloadLibraryVersionDTO> ConvertFileFormat(DownloadLibraryVersionDTO fileToConvert, string format)
    {
        SevenZipBase.SetLibraryPath($"{AppContext.BaseDirectory}7z.dll");

        var oldFileName = Path.GetFileNameWithoutExtension(fileToConvert.FileName);
        var newFileName = $"{oldFileName}.{format.TrimStart('.')}";

        var tempDirectoryPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempDirectoryPath);
        var tempFilePath = Path.Combine(tempDirectoryPath, oldFileName!);
        var newFilePath = Path.Combine(tempDirectoryPath, newFileName);

        // Write the original file to a temporary location
        await File.WriteAllBytesAsync(tempFilePath, fileToConvert.FileContent);

        // Convert the archive format
        using (var extractor = new SevenZipExtractor(tempFilePath))
        {
            const CompressionLevel compressionLevel = CompressionLevel.Normal; // Set the desired compression level
            var formatOut = format.ToLower() switch
            {
                ".zip" => OutArchiveFormat.Zip,
                ".tar" => OutArchiveFormat.Tar,
                ".gzip" => OutArchiveFormat.GZip,
                ".7z" => OutArchiveFormat.SevenZip,
                _ => throw new NotSupportedException("Unsupported format")
            };

           using (var compressor = new SevenZipCompressor())
        {
            compressor.CompressionLevel = compressionLevel;
            compressor.ArchiveFormat = formatOut;
            compressor.CompressFiles(newFilePath, tempFilePath);
        }
        }

        // Read the converted file
        var convertedFileContent = await File.ReadAllBytesAsync(newFilePath);

        // Cleanup temporary files
        File.Delete(tempFilePath);
        File.Delete(newFilePath);
        Directory.Delete(tempDirectoryPath);

        // Return the converted file
        return new DownloadLibraryVersionDTO
        {
            FileContent = convertedFileContent,
            ContentType = fileToConvert.ContentType, // You may want to update this based on the format
            FileName = newFileName
        };
    }*/
    
    private Task<DownloadLibraryVersionDTO> 
        ConvertFileFormat(DownloadLibraryVersionDTO fileToConvert, string format)
    {
        using var memoryStream = new MemoryStream();

        var writerOptions = new WriterOptions(CompressionType.None);
        var archiveType = ArchiveType.Zip;
        
        switch (format.ToLower())
        {
            case ".zip":
                writerOptions = new WriterOptions(CompressionType.Deflate);
                break;
            case ".tar":
                archiveType = ArchiveType.Tar;
                break;
            case ".gzip":
                writerOptions = new WriterOptions(CompressionType.GZip);
                archiveType = ArchiveType.GZip;
                break;
            case ".bzip2":
                writerOptions = new WriterOptions(CompressionType.BZip2);
                archiveType = ArchiveType.Tar;
                break;
            default:
                throw new NotSupportedException("Unsupported format");
        }

        var oldFileName = Path.GetFileNameWithoutExtension(fileToConvert.FileName);
        using (var archive = ArchiveFactory.Create(archiveType))
        {
            using (var fileStream = new MemoryStream(fileToConvert.FileContent))
            {
                archive.AddEntry(oldFileName!, fileStream, false, fileToConvert.FileContent.Length);
                archive.SaveTo(memoryStream, writerOptions);
            }
        }

        var newFormatExtension = format.TrimStart('.');
        return Task.FromResult(new DownloadLibraryVersionDTO
        {
            FileContent = memoryStream.ToArray(),
            ContentType = fileToConvert.ContentType,
            FileName = $"{oldFileName}.{newFormatExtension}"
        });
    }
    
    public async Task<PagedList<LibraryVersionDTO>> GetLibraryVersionsAsync(LibraryVersionParams libraryParams)
    {
        var libraryVersions = await _libraryVersionRepository.GetAllAsync(libraryParams);
        
        return new PagedList<LibraryVersionDTO>(Mapper.Map<IEnumerable<LibraryVersionDTO>>(libraryVersions.Items), 
            libraryVersions.TotalCount, libraryVersions.CurrentPage, libraryVersions.PageSize);
    }

    public async Task<ICollection<LibraryVersionDTO>> GetLibraryVersionsAsync()
    {
        var libraryVersions = await _libraryVersionRepository.GetAllAsync();
        return Mapper.Map<ICollection<LibraryVersionDTO>>(libraryVersions);
    }

    public async Task<LibraryVersionDTO> GetLibraryVersionByIdAsync(int id)
    {
        var library = await _libraryVersionRepository.GetByIdAsync(id);
        if (library is null)
        {
            throw new NotFoundException(nameof(LibraryVersion), id);
        }
        
        return Mapper.Map<LibraryVersionDTO>(library);
    }
    
    public async Task<LibraryVersionDTO> CreateLibraryVersionAsync(NewLibraryVersionDTO newLibraryVersionDto)
    {
        if (newLibraryVersionDto is null)
        {
            throw new ArgumentException("Library version data cannot be null.");
        }
        
        var library = await _libraryRepository.GetByIdAsync(newLibraryVersionDto.LibraryId);
        if (library is null)
        {
            throw new NotFoundException(nameof(Library), newLibraryVersionDto.LibraryId);
        }

        if (library.UserId != _userDataGetter.CurrentUserId)
        {
            throw new UnauthorizedAccessException("You are not allowed to create a new library version for this library.");
        }
        
        await using var transaction = await UnitOfWork.BeginTransactionAsync();
        
        try
        {
            await ValidateArchiveAsync(newLibraryVersionDto, library.Name);

            var libraryVersionEntity = await CreateLibraryVersionEntityAsync(newLibraryVersionDto);
            await _libraryVersionRepository.AddAsync(libraryVersionEntity);
            var fileHashEntity = await CreateFileHashEntityAsync(newLibraryVersionDto.LibraryVersionArchive);
            await _fileHashRepository.AddAsync(fileHashEntity);
            
            await UnitOfWork.SaveAsync();

            libraryVersionEntity = await _libraryVersionRepository.GetByIdAsync(libraryVersionEntity.Id);
            await CreateDependencies(newLibraryVersionDto.LibraryVersionArchive, libraryVersionEntity);
            await CreateLibrarySupportedFrameworks(newLibraryVersionDto.LibraryVersionArchive, 
                libraryVersionEntity.Id);

            await UploadArchiveToFirebaseStorage(newLibraryVersionDto, library.Name);
            
            await UnitOfWork.CommitTransactionAsync();
            libraryVersionEntity = await _libraryVersionRepository.GetByIdAsync(libraryVersionEntity.Id);
            return Mapper.Map<LibraryVersionDTO>(libraryVersionEntity);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unexpected error occurred while creating a new library version");
            
            var archiveStoragePath = 
                GetLibraryVersionArchiveStoragePath(newLibraryVersionDto.LibraryVersionArchive.FileName, 
                    newLibraryVersionDto.Name, library.Name);
            await _firebaseStorageService.RemoveAsync(archiveStoragePath);
            
            await UnitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
    
    private async Task<LibraryArchiveHash> CreateFileHashEntityAsync(IFormFile libraryArchive)
    {
        await using var fileStream = libraryArchive.OpenReadStream();
        var fileHash = await _fileUtilityService.CalculateFileHashAsync(fileStream);
        var fileHashEntity = new LibraryArchiveHash { Hash = fileHash };
        return fileHashEntity;
    }

    private async Task CreateLibrarySupportedFrameworks(IFormFile libraryVersionArchive, int libraryVersionId)
    {
        var frameworks = (await _frameworkRepository.GetAllAsync()).ToList();

        await using var archiveStream = libraryVersionArchive.OpenReadStream();
        var supportedFrameworksData = 
            await _libraryArchiveExtractorService.ExtractSupportedFrameworks(archiveStream);
        
        foreach (var sf in supportedFrameworksData)
        {
            await ProcessLibrarySupportedFramework(sf, frameworks, libraryVersionId);
        }
    }

    private async Task ProcessLibrarySupportedFramework(ExtractedSupportedFrameworkDTO frameworkData, 
        IEnumerable<Framework> frameworks, int libraryVersionId)
    {
        var framework = frameworks.FirstOrDefault(f => f.VersionName == frameworkData.VersionName);
        if (framework == null)
        {
            throw new NotFoundException($"Framework with version {frameworkData.VersionName} not found.");
        }
        
        var librarySupportedFramework = new NewLibraryVersionSupportedFrameworkDTO
        {
            LibraryVersionId = libraryVersionId,
            FrameworkId = framework.Id
        };

        await _libraryVersionSupportedFrameworkService
            .CreateLibraryVersionSupportedFrameworkAsync(librarySupportedFramework);
    }
    
    private string GetLibraryVersionArchiveStoragePath(string archiveName, string libraryVersion, string libraryName)
    {
        var archiveExtension = Path.GetExtension(archiveName);
        return $"{_userDataGetter.CurrentUsername}/{libraryName}/{libraryName}_{libraryVersion}{archiveExtension}";
    }
    
    private async Task UploadArchiveToFirebaseStorage(NewLibraryVersionDTO newLibraryVersionDto, string libraryName)
    {
        var archive = newLibraryVersionDto.LibraryVersionArchive;
        var libraryVersionArchiveStoragePath = 
            GetLibraryVersionArchiveStoragePath(archive.FileName, newLibraryVersionDto.Name, libraryName);

        try
        {
            await using var archiveStream = archive.OpenReadStream();
            await _firebaseStorageService.UploadAsync(archiveStream, libraryVersionArchiveStoragePath, 
                archive.ContentType);
        }
        catch (Exception)
        {
            await _firebaseStorageService.RemoveAsync(libraryVersionArchiveStoragePath);
            throw;
        }
    }

    private async Task<LibraryVersion> CreateLibraryVersionEntityAsync(NewLibraryVersionDTO newLibraryVersionDto)
    {
        var libraryVersionEntity = Mapper.Map<LibraryVersion>(newLibraryVersionDto);
        libraryVersionEntity.LastUpdateTime = DateTime.Now;
        
        var archiveExtension = Path.GetExtension(newLibraryVersionDto.LibraryVersionArchive.FileName);
        var extensionEntity = await _fileExtensionRepository.GetFileExtensionByName(archiveExtension);
        
        if (extensionEntity is null)
        {
            throw new NotFoundException($"File extension '{archiveExtension}' not found.");
        }
        libraryVersionEntity.FileExtensionId = extensionEntity.Id;
        
        return libraryVersionEntity;
    }

    private async Task CreateDependencies(IFormFile libraryVersionArchive, LibraryVersion libraryVersion)
    {
        await using var archiveStream = libraryVersionArchive.OpenReadStream();
        var dependenciesData = 
            await _libraryArchiveExtractorService.ExtractDependencies(archiveStream);
        var frameworks = (await _frameworkRepository.GetAllAsync()).ToList();

        foreach (var d in dependenciesData)
        {
            await ProcessDependency(d, frameworks, libraryVersion);
        }
    }
    
    private async Task ProcessDependency(ArchiveDependencyDTO archiveDependency, IEnumerable<Framework> frameworks, 
        LibraryVersion libraryVersion)
    {
        var framework = frameworks.FirstOrDefault(f => f.VersionName == archiveDependency.FrameworkVersion);
        if (framework is null)
        {
            throw new NotFoundException
                ($"Framework with version {archiveDependency.FrameworkVersion} not found.");
        }
        
        var newDependency = new NewLibraryVersionDependencyDTO
        {
            DependencyLibraryId = libraryVersion.Library.Id,
            SupportedVersions = $"(>= {archiveDependency.LibraryVersion})",
            FrameworkId = framework.Id,
            LibraryVersionId = libraryVersion.Id
        };

        await _libraryVersionDependencyService.CreateLibraryVersionDependencyAsync(newDependency);
    }

    private async Task ValidateArchiveAsync(NewLibraryVersionDTO newLibraryVersionDto, string libraryName)
    {
        var archive = newLibraryVersionDto.LibraryVersionArchive;
        await using var archiveStream = archive.OpenReadStream();
        
        var isArchiveValid = await _libraryArchiveValidationService
            .ValidateArchiveAsync(archiveStream, archive.FileName, libraryName, 
                newLibraryVersionDto.Name);
        if (!isArchiveValid)
        {
            throw new InvalidArchiveException();
        }
    }
    
    public async Task UpdateLibraryVersionAsync(UpdateLibraryVersionDTO updateLibraryVersionDto)
    {
        var libraryVersionEntity = await _libraryVersionRepository.GetByIdAsync(updateLibraryVersionDto.Id);
        if (libraryVersionEntity is null)
        {
            throw new NotFoundException(nameof(LibraryVersion), updateLibraryVersionDto.Id);
        }
        
        if (libraryVersionEntity.Library.UserId != _userDataGetter.CurrentUserId)
        {
            throw new UnauthorizedAccessException("You are not allowed to update this library version.");
        }
        
        libraryVersionEntity.LastUpdateTime = DateTime.UtcNow;
        libraryVersionEntity.UsageContent = updateLibraryVersionDto.UsageContent;
        libraryVersionEntity.SourceRepositoryURL = updateLibraryVersionDto.SourceRepositoryUrl;
        libraryVersionEntity.LicenseURL = updateLibraryVersionDto.LicenseUrl;
        
        await UnitOfWork.SaveAsync();
    }

    public async Task IncrementLibraryVersionDownloadsAsync(int libraryVersionId)
    {
        var libraryVersionEntity = await _libraryVersionRepository.GetByIdAsync(libraryVersionId);
        if (libraryVersionEntity is null)
        {
            throw new NotFoundException($"Library version with ID {libraryVersionId} not found.");
        }
        
        libraryVersionEntity.DownloadsCount++;
        if (libraryVersionEntity.LibraryId != null)
        {
            libraryVersionEntity.Library.DownloadsCount++;
        }
        
        await UnitOfWork.SaveAsync();
    }

    public async Task DeleteLibraryVersionAsync(int id)
    { 
        await using var transaction = await UnitOfWork.BeginTransactionAsync();
        
        try
        {
            var libraryVersionEntity = await _libraryVersionRepository.GetByIdAsync(id);
            if (libraryVersionEntity is null)
            {
                throw new NotFoundException(nameof(LibraryVersion), id);
            }

            foreach (var dependency in libraryVersionEntity.LibraryVersionDependencies)
            {
                await _libraryVersionDependencyService.DeleteLibraryVersionDependencyAsync(dependency.Id);
            }

            foreach (var librarySupportedFramework in libraryVersionEntity.LibraryVersionSupportedFrameworks)
            {
                await _libraryVersionSupportedFrameworkService
                    .DeleteLibraryVersionSupportedFrameworkAsync(librarySupportedFramework.Id);
            }

            _libraryVersionRepository.Delete(libraryVersionEntity);
            await RemoveLibraryArchiveFromStorage(libraryVersionEntity);
            
            await UnitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await UnitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    private async Task RemoveLibraryArchiveFromStorage(LibraryVersion libraryVersionEntity)
    {
        var libraryOwnerUsername = libraryVersionEntity.Library.User.Username;
        var libraryArchiveName = $"{libraryVersionEntity.Library.Name}_{libraryVersionEntity.Name}" +
                                 $"{libraryVersionEntity.FileExtension.Name}";
        var libraryArchiveStoragePath = $"{libraryOwnerUsername}/{libraryVersionEntity.Library.Name}" +
                                        $"/{libraryArchiveName}";
        await _firebaseStorageService.RemoveAsync(libraryArchiveStoragePath);
    }
}