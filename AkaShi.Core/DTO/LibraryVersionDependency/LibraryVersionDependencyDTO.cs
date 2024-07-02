using AkaShi.Core.DTO.Framework;
using AkaShi.Core.DTO.Library;
using AkaShi.Core.DTO.LibraryVersion;

namespace AkaShi.Core.DTO.LibraryVersionDependency;

public class LibraryVersionDependencyDTO
{
    public int Id { get; set; }
    public FrameworkDTO Framework { get; set; }
    public LibraryVersionDTO LibraryVersion { get; set; }
    public LibraryDTO DependencyLibrary { get; set; }
    public string SupportedVersions { get; set; }
}