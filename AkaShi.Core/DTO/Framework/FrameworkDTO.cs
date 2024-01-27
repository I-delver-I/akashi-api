using AkaShi.Core.DTO.LibraryVersionDependency;
using AkaShi.Core.DTO.LibraryVersionSupportedFramework;

namespace AkaShi.Core.DTO.Framework;

public class FrameworkDTO
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public string VersionName { get; set; }
    
    public ICollection<LibraryVersionDependencyDTO> LibraryVersionDependencies { get; private set; }
    public ICollection<LibraryVersionSupportedFrameworkDTO> LibraryVersionSupportedFrameworks { get; private set; }
}