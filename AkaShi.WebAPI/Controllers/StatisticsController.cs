using AkaShi.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AkaShi.WebAPI.Controllers;

[Route("api/[controller]")]
[AllowAnonymous]
[ApiController]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsService _statisticsService;
    
    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }
    
    [HttpGet("top-downloads")]
    public async Task<IActionResult> GetTopDownloads([FromQuery] int count = 100)
    {
        return Ok(await _statisticsService.GetTopDownloadsAsync(count));
    }
}