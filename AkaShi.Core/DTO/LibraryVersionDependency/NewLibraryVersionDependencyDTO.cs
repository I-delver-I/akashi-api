namespace AkaShi.Core.DTO.LibraryVersionDependency;

public class NewLibraryVersionDependencyDTO
{
    public int FrameworkId { get; set; }
    public int LibraryVersionId { get; set; }
    public int DependencyLibraryId { get; set; }
    public string SupportedVersions { get; set; }
}