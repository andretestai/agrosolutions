using AlertService.DTOs;

namespace AgroSolution.Tests;
public class AlertServiceTests
{
    private readonly AlertService.Services.AlertService _alertService;

    public AlertServiceTests()
    {
        _alertService = new AlertService.Services.AlertService();
    }

    [Fact]
    public void ProcessSensorData_UmidadeBaixa_DeveGerarAlertaDeSeca()
    {
        // Arrange
        var fieldId = Guid.NewGuid();
        var data = new SensorDataEvent(
            FieldId: fieldId,
            SoilHumidity: 20.0,
            Temperature: 32.0,
            Precipitation: 0.0,
            RecordedAt: DateTime.UtcNow  // mudou aqui
        );

        // Act
        _alertService.ProcessSensorData(data);
        var alerts = _alertService.GetByField(fieldId);

        // Assert
        Assert.NotEmpty(alerts);
        Assert.Contains(alerts, a => a.Type == "Alerta de Seca");
    }

    [Fact]
    public void ProcessSensorData_UmidadeNormal_NaoDeveGerarAlerta()
    {
        // Arrange
        var fieldId = Guid.NewGuid();
        var data = new SensorDataEvent(
            FieldId: fieldId,
            SoilHumidity: 60.0,
            Temperature: 25.0,
            Precipitation: 10.0,
            RecordedAt: DateTime.UtcNow
        );

        // Act
        _alertService.ProcessSensorData(data);
        var alerts = _alertService.GetByField(fieldId);

        // Assert
        Assert.Empty(alerts);
    }

    [Fact]
    public void ProcessSensorData_TemperaturaAlta_DeveGerarRiscoDePraga()
    {
        // Arrange
        var fieldId = Guid.NewGuid();
        var data = new SensorDataEvent(
            FieldId: fieldId,
            SoilHumidity: 50.0,
            Temperature: 41.0,
            Precipitation: 0.0,
            RecordedAt: DateTime.UtcNow
        );

        // Act
        _alertService.ProcessSensorData(data);
        var alerts = _alertService.GetByField(fieldId);

        // Assert
        Assert.NotEmpty(alerts);
        Assert.Contains(alerts, a => a.Type == "Risco de Praga");
    }

    [Fact]
    public void ProcessSensorData_AlertaDuplicado_NaoDeveDuplicarAlerta()
    {
        // Arrange
        var fieldId = Guid.NewGuid();
        var data = new SensorDataEvent(
            FieldId: fieldId,
            SoilHumidity: 20.0,
            Temperature: 41.0,
            Precipitation: 0.0,
            RecordedAt: DateTime.UtcNow  // mudou aqui
        );

        // Act
        _alertService.ProcessSensorData(data);
        _alertService.ProcessSensorData(data);
        var alerts = _alertService.GetByField(fieldId);

        // Assert
        Assert.Single(alerts.Where(a => a.Type == "Alerta de Seca"));
        Assert.Single(alerts.Where(a => a.Type == "Risco de Praga"));
    }
}