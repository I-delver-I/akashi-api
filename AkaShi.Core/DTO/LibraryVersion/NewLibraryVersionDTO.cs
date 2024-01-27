using Microsoft.AspNetCore.Http;

namespace AkaShi.Core.DTO.LibraryVersion;

public class NewLibraryVersionDTO
{
    public string Name { get; set; }
    public int LibraryId { get; set; }
    public IFormFile LibraryVersionArchive { get; set; }
}