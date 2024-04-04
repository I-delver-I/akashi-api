using AkaShi.Core.DTO.LibraryVersion;
using AkaShi.Core.DTO.LibraryVersionDependency;
using AkaShi.Core.Helpers.RepositoryParams;
using AkaShi.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AkaShi.WebAPI.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class LibraryVersionsController : ControllerBase
{
    private readonly ILibraryVersionService _libraryVersionService;

    public LibraryVersionsController(ILibraryVersionService libraryVersionService)
    {
        _libraryVersionService = libraryVersionService;
    }

    [HttpGet("dependencies/{libraryVersionId:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<ICollection<LibraryVersionDependencyDTO>>> GetDependencies(int libraryVersionId)
    {
        var dependencies = await _libraryVersionService.GetLibraryVersionDependenciesAsync(libraryVersionId);
        return Ok(dependencies);
    }
    
    [HttpGet("by-library/{libraryId:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<ICollection<LibraryVersionDTO>>> GetByLibraryId(int libraryId)
    {
        var libraryVersions = 
            await _libraryVersionService.GetLibraryVersionsByLibraryIdAsync(libraryId);
    
        return Ok(libraryVersions);
    }

    
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ICollection<LibraryVersionDTO>>> Get([FromHeader] int pageSize = 10, 
        [FromHeader] int pageNumber = 1)
    {
        var libraryVersionParams = new LibraryVersionParams
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        
        return Ok(await _libraryVersionService.GetLibraryVersionsAsync(libraryVersionParams));
    }
    
    [HttpGet("download/{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> DownloadLibraryVersion(int id, [FromHeader] string format)
    {
        var downloadLibraryVersion = await _libraryVersionService.DownloadLibraryVersionAsync(id, format);
        return File(downloadLibraryVersion.FileContent, downloadLibraryVersion.ContentType, 
            downloadLibraryVersion.FileName);
    }
    
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<LibraryVersionDTO>> GetById(int id)
    {
        var libraryVersion = await _libraryVersionService.GetLibraryVersionByIdAsync(id);
        return Ok(libraryVersion);
    }
    
    [HttpPost]
    public async Task<ActionResult<LibraryVersionDTO>> Post([FromForm] NewLibraryVersionDTO libraryVersionDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var createdLibraryVersion = await _libraryVersionService.CreateLibraryVersionAsync(libraryVersionDto); 
        return CreatedAtAction(nameof(GetById), 
            new { id = createdLibraryVersion.Id }, createdLibraryVersion);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _libraryVersionService.DeleteLibraryVersionAsync(id);
        return NoContent();
    }
}