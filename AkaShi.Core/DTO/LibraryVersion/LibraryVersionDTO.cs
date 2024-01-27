using AkaShi.Core.DTO.Library;
using AkaShi.Core.DTO.LibraryVersionDependency;
using AkaShi.Core.DTO.LibraryVersionSupportedFramework;

namespace AkaShi.Core.DTO.LibraryVersion;

public class LibraryVersionDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int DownloadsCount { get; set; }
    public LibraryDTO Library { get; set; }
    public DateTime LastUpdateTime { get; set; }
    public string UsageContent { get; set; }
    public string SourceRepositoryUrl { get; set; }
    public string LicenseUrl { get; set; }
    public string FileExtension { get; set; }
    
    public ICollection<LibraryVersionDependencyDTO> LibraryVersionDependencies { get; private set; }
    public ICollection<LibraryVersionSupportedFrameworkDTO> LibraryVersionSupportedFrameworks { get; private set; }
}