    using AkaShi.Core.Common;
    using AkaShi.Core.ServiceContracts;
    using AkaShi.Core.Domain.RepositoryContracts;
    using AkaShi.Core.Services.Abstract;
    using AutoMapper;
    using Serilog;
    using SharpCompress.Archives;

    namespace AkaShi.Infrastructure.Services;

    public class LibraryArchiveValidationService : BaseService, ILibraryArchiveValidationService
    {
        private readonly HashSet<string> _allowedArchiveExtensions = new() { ".zip", ".rar", ".tar", ".gzip", ".7z" };

        private readonly IFrameworkRepository _frameworkRepository;
        
        private readonly IDllValidationService _dllValidationService;
        private readonly IFileUtilityService _fileUtilityService;
        
        public LibraryArchiveValidationService(IUnitOfWork unitOfWork, IMapper mapper, 
            IDllValidationService dllValidationService, IFileUtilityService fileUtilityService) 
            : base(unitOfWork, mapper)
        {
            _frameworkRepository = UnitOfWork.FrameworkRepository;
            
            _dllValidationService = dllValidationService;
            _fileUtilityService = fileUtilityService;
        }
        
        public async Task<bool> ValidateArchiveAsync(Stream archiveStream, string archiveFullName,
            string libraryName, string libraryVersionName)
        {
            var buffer = new MemoryStream();
            await archiveStream.CopyToAsync(buffer);
            
            if (!IsAllowedExtension(archiveFullName))
            {
                Log.Warning("Invalid file extension for {FileName}", archiveFullName);
                return false;
            }

            buffer.Position = 0;
            if (!IsFileSizeWithinLimit(buffer))
            {
                Log.Warning("Archive size is too large for {FileName}", archiveFullName);
                return false;
            }
            
            buffer.Position = 0;
            if (await IsDuplicateArchive(buffer))
            {
                Log.Error("Duplicate archive detected for {FileName}", archiveFullName);
                return false;
            }
            
            buffer.Position = 0;
            return await AreEntriesValid(buffer, libraryName);
        }

        private async Task<bool> AreEntriesValid(Stream archiveStream, string libraryName)
        {
            var frameworkVersionNames = (await _frameworkRepository.GetAllAsync())
                .Select(fv => fv.VersionName).ToList();
                
            using var archive = ArchiveFactory.Open(archiveStream);
            foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
            {
                if (!await IsValidEntry(entry, libraryName, frameworkVersionNames))
                {
                    return false;
                }
            }

            return true;
        }
        
        private async Task<bool> IsDuplicateArchive(Stream archiveStream)
        {
            archiveStream.Position = 0;
            var fileHash = await _fileUtilityService.CalculateFileHashAsync(archiveStream);
            return await _fileUtilityService.HashExistsAsync(fileHash);
        }

        private bool IsFileSizeWithinLimit(Stream archiveStream)
        {
            const int bytesInMb = 1024 * 1024;
            const long maxArchiveAllowedSizeMb = 250 * bytesInMb;
            if (archiveStream.Length <= maxArchiveAllowedSizeMb)
            {
                return true;
            }
            
            Log.Warning("The archive size is too big: {ArchiveSize} bytes", 
                archiveStream.Length);
            return false;
        }

        private bool IsAllowedExtension(string archiveFullName)
        {
            var archiveExtension = Path.GetExtension(archiveFullName).ToLowerInvariant();
            if (_allowedArchiveExtensions.Contains(archiveExtension))
            {
                return true;
            }
            
            Log.Warning("The file extension \'{FileExtension}\' is not allowed", 
                archiveExtension);
            return false;
        }
        
        private async Task<bool> IsValidEntry(IArchiveEntry entry, string libraryName, 
            IEnumerable<string> frameworkVersionNames)
        {
            var parts = entry.Key.Split(@"\");
            if (!IsValidEntryStructure(parts, frameworkVersionNames))
            {
                return false;
            }
            
            return await IsValidDllEntry(entry, libraryName);
        }
        
        private bool IsValidEntryStructure(IReadOnlyList<string> parts, IEnumerable<string> frameworkVersionNames)
        {
            if (parts.Count != 3 || parts[0] != ApplicationConstants.LibFolderName)
            {
                return false;
            }
            
            return frameworkVersionNames.Contains(parts[1]);
        }
        
        private async Task<bool> IsValidDllEntry(IArchiveEntry entry, string libraryName)
        {
            var fullFileName = Path.GetFileName(entry.Key);
            var correctFullLibraryName = $"{libraryName}{ApplicationConstants.DllExtension}";
            if (fullFileName != correctFullLibraryName)
            {
                return false;
            }

            await using var entryStream = entry.OpenEntryStream();
            await using var memoryStream = new MemoryStream();
            await entryStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            
            return await _dllValidationService.IsCsharpDll(memoryStream);
        }
    }