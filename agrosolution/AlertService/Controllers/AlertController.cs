using AlertService.DTOs;
using AlertService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlertService.Controllers;

[ApiController]
[Route("api/alerts")]
public class AlertController : ControllerBase
{
    private readonly AlertService.Services.AlertService _alertService;

    public AlertController(AlertService.Services.AlertService alertService)
    {
        _alertService = alertService;
    }

    /// <summary>Simula recebimento de dados do sensor (normalmente viria do RabbitMQ)</summary>
    [HttpPost("process")]
    public IActionResult ProcessSensorData([FromBody] SensorDataEvent data)
    {
        _alertService.ProcessSensorData(data);
        return Ok(new { message = "Dados processados com sucesso." });
    }

    /// <summary>Retorna todos os alertas ativos</summary>
    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _alertService.GetAll();
        return Ok(result);
    }

    /// <summary>Retorna alertas ativos de um talhão específico</summary>
    [HttpGet("{fieldId}")]
    public IActionResult GetByField(Guid fieldId)
    {
        var result = _alertService.GetByField(fieldId);
        return Ok(result);
    }
}