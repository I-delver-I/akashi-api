namespace AkaShi.Core.DTO.LibraryVersion;

public class DownloadLibraryVersionDTO
{
    public byte[] FileContent { get; set; }
    public string ContentType { get; set; }
    public string FileName { get; set; }
}