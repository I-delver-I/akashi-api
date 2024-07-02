using AkaShi.Core.DTO.Framework;
using AkaShi.Core.DTO.LibraryVersion;

namespace AkaShi.Core.DTO.LibraryVersionSupportedFramework;

public class LibraryVersionSupportedFrameworkDTO
{
    public int Id { get; set; }
    public LibraryVersionDTO LibraryVersion { get; set; }
    public FrameworkDTO Framework { get; set; }
}