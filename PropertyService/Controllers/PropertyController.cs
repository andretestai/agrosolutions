using Microsoft.AspNetCore.Mvc;
using PropertyService.DTOs;
using PropertyService.Services;

namespace PropertyService.Controllers;

[ApiController]
[Route("api/properties")]
public class PropertyController : ControllerBase
{
    private readonly PropertyService.Services.PropertyService _propertyService;

    public PropertyController(PropertyService.Services.PropertyService propertyService)
    {
        _propertyService = propertyService;
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreatePropertyRequest req)
    {
        var ownerId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var result = _propertyService.Create(ownerId, req);
        return Created(string.Empty, result);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var ownerId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var result = _propertyService.GetByOwner(ownerId);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var ownerId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var result = _propertyService.GetById(id, ownerId);
        if (result is null) return NotFound();

        return Ok(result);
    }

    [HttpPost("{propertyId}/fields")]
    public IActionResult AddField(Guid propertyId, [FromBody] CreateFieldRequest req)
    {
        var ownerId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var result = _propertyService.AddField(propertyId, ownerId, req);
        if (result is null) return NotFound(new { message = "Propriedade não encontrada." });

        return Created(string.Empty, result);
    }

    [HttpGet("{propertyId}/fields")]
    public IActionResult GetFields(Guid propertyId)
    {
        var ownerId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var result = _propertyService.GetFields(propertyId, ownerId);
        return Ok(result);
    }
}