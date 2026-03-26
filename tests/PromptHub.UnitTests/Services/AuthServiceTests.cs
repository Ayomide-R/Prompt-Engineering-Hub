using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using PromptHub.Application.Interfaces;
using PromptHub.Application.Services;
using PromptHub.Domain.Entities;
using PromptHub.UnitTests.Helpers;
using Xunit;

namespace PromptHub.UnitTests.Services;

public class AuthServiceTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IJwtTokenGenerator> _tokenGeneratorMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _tokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        _authService = new AuthService(_contextMock.Object, _passwordHasherMock.Object, _tokenGeneratorMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnToken_WhenUserDoesNotExist()
    {
        // Arrange
        var users = new List<User>().AsQueryable();
        var mockDbSet = users.BuildMockDbSet();
        _contextMock.Setup(c => c.Users).Returns(mockDbSet.Object);
        _passwordHasherMock.Setup(h => h.Hash(It.IsAny<string>())).Returns("hashed_password");
        _tokenGeneratorMock.Setup(g => g.GenerateToken(It.IsAny<User>())).Returns("test_token");

        // Act
        var result = await _authService.RegisterAsync("testuser", "test@example.com", "password123");

        // Assert
        result.Token.Should().Be("test_token");
        result.Email.Should().Be("test@example.com");
        result.Username.Should().Be("testuser");
        _contextMock.Verify(c => c.Users.Add(It.IsAny<User>()), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowException_WhenUserExists()
    {
        // Arrange
        var users = new List<User> { new User { Email = "test@example.com" } }.AsQueryable();
        var mockDbSet = users.BuildMockDbSet();
        _contextMock.Setup(c => c.Users).Returns(mockDbSet.Object);

        // Act & Assert
        await _authService.Invoking(s => s.RegisterAsync("testuser", "test@example.com", "password123"))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("User with this email already exists.");
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var user = new User { Email = "test@example.com", PasswordHash = "hashed_password" };
        var users = new List<User> { user }.AsQueryable();
        var mockDbSet = users.BuildMockDbSet();
        _contextMock.Setup(c => c.Users).Returns(mockDbSet.Object);
        _passwordHasherMock.Setup(h => h.Verify("password123", "hashed_password")).Returns(true);
        _tokenGeneratorMock.Setup(g => g.GenerateToken(user)).Returns("test_token");

        // Act
        var result = await _authService.LoginAsync("test@example.com", "password123");

        // Assert
        result.Token.Should().Be("test_token");
        result.Email.Should().Be("test@example.com");
        result.Username.Should().Be("testuser");
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowException_WhenCredentialsAreInvalid()
    {
        // Arrange
        var user = new User { Email = "test@example.com", PasswordHash = "hashed_password" };
        var users = new List<User> { user }.AsQueryable();
        var mockDbSet = users.BuildMockDbSet();
        _contextMock.Setup(c => c.Users).Returns(mockDbSet.Object);
        _passwordHasherMock.Setup(h => h.Verify("wrong_password", "hashed_password")).Returns(false);

        // Act & Assert
        await _authService.Invoking(s => s.LoginAsync("test@example.com", "wrong_password"))
            .Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Invalid email or password.");
    }
}
