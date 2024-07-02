using AkaShi.Core.DTO.LibraryVersionDependency;
using AkaShi.Core.DTO.LibraryVersionSupportedFramework;

namespace AkaShi.Core.ServiceContracts;

public interface ILibraryArchiveExtractorService
{
    Task<List<ArchiveDependencyDTO>> ExtractDependencies(Stream libraryVersionArchiveFile);
    Task<List<ExtractedSupportedFrameworkDTO>> ExtractSupportedFrameworks(Stream libraryVersionArchiveFile);
}