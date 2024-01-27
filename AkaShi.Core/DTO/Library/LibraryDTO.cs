using AkaShi.Core.DTO.LibraryVersion;
using AkaShi.Core.DTO.LibraryVersionDependency;
using AkaShi.Core.DTO.User;

namespace AkaShi.Core.DTO.Library;

public class LibraryDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public UserDTO User { get; set; }
    public int DownloadsCount { get; set; }
    public DateTime LastUpdateTime { get; set; }
    public string ShortDescription { get; set; }
    public string Tags { get; set; }
    public string ProjectWebsiteURL { get; set; }
    public string LogoURL { get; set; }
    
    public ICollection<LibraryVersionDTO> LibraryVersions { get; private set; }
    public ICollection<LibraryVersionDependencyDTO> LibraryVersionDependencies { get; private set; }
}