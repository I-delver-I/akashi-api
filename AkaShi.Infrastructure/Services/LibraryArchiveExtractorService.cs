using System.Reflection;
using AkaShi.Core.Common;
using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts;
using AkaShi.Core.DTO.LibraryVersionDependency;
using AkaShi.Core.DTO.LibraryVersionSupportedFramework;
using AkaShi.Core.Exceptions;
using AkaShi.Core.ServiceContracts;
using AkaShi.Core.Services.Abstract;
using AutoMapper;
using SharpCompress.Archives;

namespace AkaShi.Infrastructure.Services;

public class LibraryArchiveExtractorService : BaseService, ILibraryArchiveExtractorService
{
    private readonly IFrameworkRepository _frameworkRepository;
    private readonly ILibraryRepository _libraryRepository;
    
    public LibraryArchiveExtractorService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
        _frameworkRepository = UnitOfWork.FrameworkRepository;
        _libraryRepository = UnitOfWork.LibraryRepository;
    }

    public async Task<List<ExtractedSupportedFrameworkDTO>> ExtractSupportedFrameworks(Stream libraryVersionArchiveFile)
    {
        var supportedFrameworks = new List<ExtractedSupportedFrameworkDTO>();
        var frameworks = (await _frameworkRepository.GetAllAsync()).ToList();
        
        using var archive = ArchiveFactory.Open(libraryVersionArchiveFile);
        foreach (var entry in archive.Entries
                     .Where(e => !e.IsDirectory && e.Key.EndsWith(ApplicationConstants.DllExtension)))
        {
            var frameworkVersion = entry.Key.Split(@"\")[1];
            
            var frameworkProductName = frameworks
                .FirstOrDefault(f => f.VersionName == frameworkVersion)?.ProductName;
            if (frameworkProductName is null)
            {
                throw new NotFoundException(nameof(Framework), frameworkVersion);
            }

            supportedFrameworks.Add(new ExtractedSupportedFrameworkDTO
            {
                ProductName = frameworkProductName,
                VersionName = frameworkVersion
            });
        }

        return supportedFrameworks;
    }
    
    public async Task<List<ArchiveDependencyDTO>> ExtractDependencies(Stream libraryVersionArchiveFile)
    {
        var dependencies = new List<ArchiveDependencyDTO>();
        var libraryNames = (await _libraryRepository.GetAllAsync()).Select(l => l.Name);
        
        using var archive = ArchiveFactory.Open(libraryVersionArchiveFile);
        foreach (var entry in archive.Entries.Where(e => !e.IsDirectory 
                                                         && e.Key.EndsWith(ApplicationConstants.DllExtension)))
        {
            var frameworkVersion = entry.Key.Split(@"\")[1];

            await using var entryStream = entry.OpenEntryStream();
            var dllDependencies = ExtractDependenciesFromDll(entryStream)
                    .Where(d => libraryNames.Contains(d.LibraryName));

            dependencies.AddRange(dllDependencies
                .Select(d => new ArchiveDependencyDTO 
                { 
                    FrameworkVersion = frameworkVersion, 
                    LibraryName = d.LibraryName, 
                    LibraryVersion = d.LibraryVersion 
                }));
        }

        return dependencies;
    }

    private IEnumerable<ExtractedDependencyDTO> ExtractDependenciesFromDll(Stream dllStream)
    {
        using var memoryStream = new MemoryStream();
        dllStream.CopyTo(memoryStream);
        var assemblyData = memoryStream.ToArray();
        
        var assembly = Assembly.Load(assemblyData);
        
        return assembly.GetReferencedAssemblies()
            .Where(a => a.Version != null)
            .Select(a => new ExtractedDependencyDTO 
            {
                LibraryVersion = $"{a.Version.Major}.{a.Version.Minor}.{a.Version.Build}", 
                LibraryName = a.Name 
            });
    }
}