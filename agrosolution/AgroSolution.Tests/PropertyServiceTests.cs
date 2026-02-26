using PropertyService.DTOs;
using PropertyService.Services;
using Xunit;

namespace AgroSolution.Tests;
public class PropertyServiceTests
{
    private readonly PropertyService.Services.PropertyService _propertyService;

    public PropertyServiceTests()
    {
        _propertyService = new PropertyService.Services.PropertyService();
    }

    [Fact]
    public void Create_NovaPropriedade_DeveRetornarPropriedadeCriada()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var req = new CreatePropertyRequest("Fazenda São João", "Interior de SP", 150.5);

        // Act
        var result = _propertyService.Create(ownerId, req);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Fazenda São João", result.Name);
        Assert.Equal("Interior de SP", result.Location);
        Assert.Equal(150.5, result.TotalAreaHectares);
        Assert.Empty(result.Fields);
    }

    [Fact]
    public void GetByOwner_PropriedadesCriadas_DeveRetornarListaCorreta()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        _propertyService.Create(ownerId, new CreatePropertyRequest("Fazenda A", "SP", 100));
        _propertyService.Create(ownerId, new CreatePropertyRequest("Fazenda B", "MG", 200));

        // Act
        var result = _propertyService.GetByOwner(ownerId);

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void GetByOwner_OutroOwner_DeveRetornarListaVazia()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var outroOwnerId = Guid.NewGuid();
        _propertyService.Create(ownerId, new CreatePropertyRequest("Fazenda A", "SP", 100));

        // Act
        var result = _propertyService.GetByOwner(outroOwnerId);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void AddField_PropriedadeExistente_DeveAdicionarTalhao()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var property = _propertyService.Create(ownerId, new CreatePropertyRequest("Fazenda A", "SP", 100));
        var req = new CreateFieldRequest("Talhão A", "Soja", 50.0);

        // Act
        var result = _propertyService.AddField(property.Id, ownerId, req);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Talhão A", result.Name);
        Assert.Equal("Soja", result.Crop);
        Assert.Equal("Normal", result.Status);
    }

    [Fact]
    public void AddField_PropriedadeInexistente_DeveRetornarNull()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var req = new CreateFieldRequest("Talhão A", "Soja", 50.0);

        // Act
        var result = _propertyService.AddField(Guid.NewGuid(), ownerId, req);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetFields_TalhoesCriados_DeveRetornarListaCorreta()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var property = _propertyService.Create(ownerId, new CreatePropertyRequest("Fazenda A", "SP", 100));
        _propertyService.AddField(property.Id, ownerId, new CreateFieldRequest("Talhão A", "Soja", 50.0));
        _propertyService.AddField(property.Id, ownerId, new CreateFieldRequest("Talhão B", "Milho", 30.0));

        // Act
        var result = _propertyService.GetFields(property.Id, ownerId);

        // Assert
        Assert.Equal(2, result.Count);
    }
}