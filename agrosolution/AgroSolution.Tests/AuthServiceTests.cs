using IdentityService.DTOs;
using IdentityService.Services;
using Xunit;

namespace AgroSolution.Tests;
public class AuthServiceTests
{
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _authService = new AuthService();
    }

    [Fact]
    public void Register_NovoUsuario_DeveRetornarToken()
    {
        // Arrange
        var req = new RegisterRequest("João Silva", $"{Guid.NewGuid()}@agro.com", "123456");

        // Act
        var result = _authService.Register(req);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Token);
        Assert.Equal("João Silva", result.Name);
    }

    [Fact]
    public void Register_EmailDuplicado_DeveRetornarNull()
    {
        // Arrange
        var email = $"{Guid.NewGuid()}@agro.com";
        var req = new RegisterRequest("João Silva", email, "123456");

        // Act
        _authService.Register(req);
        var result = _authService.Register(req);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Login_CredenciaisCorretas_DeveRetornarToken()
    {
        // Arrange
        var email = $"{Guid.NewGuid()}@agro.com";
        var req = new RegisterRequest("João Silva", email, "123456");
        _authService.Register(req);

        // Act
        var result = _authService.Login(new LoginRequest(email, "123456"));

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Token);
    }

    [Fact]
    public void Login_SenhaErrada_DeveRetornarNull()
    {
        // Arrange
        var email = $"{Guid.NewGuid()}@agro.com";
        _authService.Register(new RegisterRequest("João Silva", email, "123456"));

        // Act
        var result = _authService.Login(new LoginRequest(email, "senhaerrada"));

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Login_EmailNaoCadastrado_DeveRetornarNull()
    {
        // Arrange
        var req = new LoginRequest("naocadastrado@agro.com", "123456");

        // Act
        var result = _authService.Login(req);

        // Assert
        Assert.Null(result);
    }
}