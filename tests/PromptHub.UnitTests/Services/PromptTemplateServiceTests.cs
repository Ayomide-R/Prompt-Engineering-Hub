using Moq;
using FluentAssertions;
using PromptHub.Application.Interfaces;
using PromptHub.Application.Services;
using PromptHub.Domain.Entities;
using PromptHub.Domain.Enums;
using PromptHub.UnitTests.Helpers;

namespace PromptHub.UnitTests.Services;

public class PromptTemplateServiceTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly PromptTemplateService _service;

    public PromptTemplateServiceTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _service = new PromptTemplateService(_contextMock.Object);
    }

    [Fact]
    public async Task UpdateTemplateAsync_ShouldReturnTrue_WhenUpdateIsSuccessful()
    {
        // Arrange
        var template = new PromptTemplate { Id = Guid.NewGuid(), Title = "Old Title" };
        var templates = new List<PromptTemplate>().AsQueryable();
        _contextMock.Setup(x => x.PromptTemplates).Returns(templates.BuildMockDbSet().Object);
        _contextMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        var result = await _service.UpdateTemplateAsync(template);

        // Assert
        result.Should().BeTrue();
        _contextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task DeleteTemplateAsync_ShouldReturnTrue_WhenTemplateExists()
    {
        // Arrange
        var templateId = Guid.NewGuid();
        var template = new PromptTemplate { Id = templateId };
        var templates = new List<PromptTemplate> { template }.AsQueryable();

        _contextMock.Setup(x => x.PromptTemplates).Returns(templates.BuildMockDbSet().Object);
        _contextMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        var result = await _service.DeleteTemplateAsync(templateId);

        // Assert
        result.Should().BeTrue();
        _contextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task DeleteTemplateAsync_ShouldReturnFalse_WhenTemplateDoesNotExist()
    {
        // Arrange
        var templateId = Guid.NewGuid();
        var templates = new List<PromptTemplate>().AsQueryable();

        _contextMock.Setup(x => x.PromptTemplates).Returns(templates.BuildMockDbSet().Object);

        // Act
        var result = await _service.DeleteTemplateAsync(templateId);

        // Assert
        result.Should().BeFalse();
        _contextMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
}
