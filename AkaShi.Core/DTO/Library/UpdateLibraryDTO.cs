using Microsoft.AspNetCore.Http;

namespace AkaShi.Core.DTO.Library;

public class UpdateLibraryDTO
{
    public int Id { get; set; }
    public string ShortDescription { get; set; }
    public string Tags { get; set; }
    public string ProjectWebsiteURL { get; set; }
    public IFormFile Logo { get; set; }
}