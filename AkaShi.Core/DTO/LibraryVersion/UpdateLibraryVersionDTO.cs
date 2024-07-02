namespace AkaShi.Core.DTO.LibraryVersion;

public class UpdateLibraryVersionDTO
{
    public int Id { get; set; }
    public string UsageContent { get; set; }
    public string SourceRepositoryUrl { get; set; }
    public string LicenseUrl { get; set; }
}