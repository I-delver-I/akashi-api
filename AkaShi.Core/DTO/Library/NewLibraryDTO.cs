using Microsoft.AspNetCore.Http;

namespace AkaShi.Core.DTO.Library;

public class NewLibraryDTO
{
    public string Name { get; set; }
    public string InitialVersionName { get; set; }
    public IFormFile InitialVersionArchive { get; set; }
}