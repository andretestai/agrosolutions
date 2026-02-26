using IngestionService.DTOs;
using IngestionService.Services;
using Microsoft.AspNetCore.Mvc;

namespace IngestionService.Controllers;

[ApiController]
[Route("api/sensors")]
public class IngestionController : ControllerBase
{
    private readonly IngestionService.Services.IngestionService _ingestionService;

    public IngestionController(IngestionService.Services.IngestionService ingestionService)
    {
        _ingestionService = ingestionService;
    }

    [HttpPost("data")]
    public async Task<IActionResult> AddReading([FromBody] SensorReadingRequest req)
    {
        var result = await _ingestionService.AddAsync(req);
        return Created(string.Empty, result);
    }

    [HttpGet("data/{fieldId}")]
    public IActionResult GetReadings(Guid fieldId)
    {
        var result = _ingestionService.GetByField(fieldId);
        return Ok(result);
    }
}