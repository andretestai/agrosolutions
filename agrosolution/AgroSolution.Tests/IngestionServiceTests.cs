using IngestionService.DTOs;
using Microsoft.Extensions.Configuration;
using Moq;

namespace AgroSolution.Tests;
public class IngestionServiceTests
{
    private readonly IngestionService.Services.IngestionService _ingestionService;

    public IngestionServiceTests()
    {
        var configMock = new Mock<IConfiguration>();
        configMock.Setup(c => c["RabbitMQ:Host"]).Returns("localhost");
        configMock.Setup(c => c["RabbitMQ:User"]).Returns("guest");
        configMock.Setup(c => c["RabbitMQ:Pass"]).Returns("guest");

        _ingestionService = new IngestionService.Services.IngestionService(configMock.Object);
    }

    [Fact]
    public async Task AddAsync_DadosValidos_DeveRetornarLeituraCriada()
    {
        // Arrange
        var fieldId = Guid.NewGuid();
        var req = new SensorReadingRequest(fieldId, 25.0, 32.0, 0.0);

        // Act
        var result = await _ingestionService.AddAsync(req);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fieldId, result.FieldId);
        Assert.Equal(25.0, result.SoilHumidity);
        Assert.Equal(32.0, result.Temperature);
    }

    [Fact]
    public async Task AddAsync_MultiplasLeituras_DeveArmazenarTodas()
    {
        // Arrange
        var fieldId = Guid.NewGuid();

        // Act
        await _ingestionService.AddAsync(new SensorReadingRequest(fieldId, 25.0, 32.0, 0.0));
        await _ingestionService.AddAsync(new SensorReadingRequest(fieldId, 30.0, 33.0, 5.0));
        var result = _ingestionService.GetByField(fieldId);

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByField_FieldIdDiferente_DeveRetornarListaVazia()
    {
        // Arrange
        var fieldId = Guid.NewGuid();
        var outroFieldId = Guid.NewGuid();
        await _ingestionService.AddAsync(new SensorReadingRequest(fieldId, 25.0, 32.0, 0.0));

        // Act
        var result = _ingestionService.GetByField(outroFieldId);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetByField_SemLeituras_DeveRetornarListaVazia()
    {
        // Arrange
        var fieldId = Guid.NewGuid();

        // Act
        var result = _ingestionService.GetByField(fieldId);

        // Assert
        Assert.Empty(result);
    }
}