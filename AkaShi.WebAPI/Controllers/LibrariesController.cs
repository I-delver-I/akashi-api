using AkaShi.Core.DTO.Library;
using AkaShi.Core.Helpers;
using AkaShi.Core.Helpers.RepositoryParams;
using AkaShi.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AkaShi.WebAPI.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class LibrariesController : ControllerBase
{
    private readonly ILibraryService _libraryService;

    public LibrariesController(ILibraryService libraryService)
    {
        _libraryService = libraryService;
    }
    
    /*[HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedList<LibraryDTO>>> Get(int pageNumber = 1, int pageSize = 10,
        [FromQuery] string? searchTerm = null)
    {
        var paginatedParams = new PaginatedParams()
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        
        return Ok(await _libraryService.GetLibrariesAsync(paginatedParams, searchTerm));
    }*/
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedList<LibraryDTO>>> Get(string frameworks = null, string q = null, 
        SortBy sortBy = SortBy.Downloads, int page = 1, [FromHeader] int pageSize = 10)
    {
        var libraryParams = new LibraryParams
        {
            PageNumber = page,
            PageSize = pageSize,
            LibrariesFilter = LibrariesFilter.ParseFromString(frameworks),
            SortBy = sortBy,
            SearchTerm = q
        };
        
        return Ok(await _libraryService.GetLibrariesAsync(libraryParams));
    }
    
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<LibraryDTO>> GetById(int id)
    {
        return Ok(await _libraryService.GetLibraryByIdAsync(id));
    }
    
    [HttpPost]
    public async Task<ActionResult<LibraryDTO>> Post([FromForm] NewLibraryDTO newLibraryDto)
    {
        return Ok(await _libraryService.CreateLibraryAsync(newLibraryDto));
    }
    
    [HttpPut]
    public async Task<IActionResult> Put([FromForm] UpdateLibraryDTO libraryDto)
    {
        await _libraryService.UpdateLibraryAsync(libraryDto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _libraryService.DeleteLibraryAsync(id);
        return NoContent();
    }
}