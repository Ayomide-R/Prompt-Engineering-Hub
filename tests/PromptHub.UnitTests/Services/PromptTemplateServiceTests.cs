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

    [Fact]
    public async Task SearchTemplatesAsync_ShouldFilterBySearchTerm_InTitleAndDescription()
    {
        // Arrange
        var templates = new List<PromptTemplate>
        {
            new PromptTemplate { Id = Guid.NewGuid(), Title = "Expert C#", Description = "Desc", IsPublic = true },
            new PromptTemplate { Id = Guid.NewGuid(), Title = "Title", Description = "Expert Java", IsPublic = true },
            new PromptTemplate { Id = Guid.NewGuid(), Title = "Other", Description = "Other", IsPublic = true }
        }.AsQueryable();

        _contextMock.Setup(x => x.PromptTemplates).Returns(templates.BuildMockDbSet().Object);

        // Act
        var result = await _service.SearchTemplatesAsync("expert", null);

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task SearchTemplatesAsync_ShouldFilterByCategory()
    {
        // Arrange
        var templates = new List<PromptTemplate>
        {
            new PromptTemplate { Id = Guid.NewGuid(), Title = "T1", Category = "Coding", IsPublic = true },
            new PromptTemplate { Id = Guid.NewGuid(), Title = "T2", Category = "Writing", IsPublic = true }
        }.AsQueryable();

        _contextMock.Setup(x => x.PromptTemplates).Returns(templates.BuildMockDbSet().Object);

        // Act
        var result = await _service.SearchTemplatesAsync(null, "Coding");

        // Assert
        result.Should().HaveCount(1);
        result.First().Category.Should().Be("Coding");
    }

    [Fact]
    public async Task UpdateTemplateAsync_ShouldArchiveVersion_BeforeUpdating()
    {
        // Arrange
        var templateId = Guid.NewGuid();
        var template = new PromptTemplate 
        { 
            Id = templateId, 
            Title = "New Title",
            Description = "New Desc",
            RequiredVariables = "Var1"
        };
        
        var currentTemplate = new PromptTemplate 
        { 
            Id = templateId, 
            Title = "Old Title",
            Description = "Old Desc",
            RequiredVariables = "Var0"
        };

        var templates = new List<PromptTemplate> { currentTemplate }.AsQueryable();
        var versions = new List<PromptTemplateVersion>().AsQueryable();
        var versionsMock = versions.BuildMockDbSet();

        _contextMock.Setup(x => x.PromptTemplates).Returns(templates.BuildMockDbSet().Object);
        _contextMock.Setup(x => x.PromptTemplateVersions).Returns(versionsMock.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        var result = await _service.UpdateTemplateAsync(template);

        // Assert
        result.Should().BeTrue();
        versionsMock.Verify(x => x.Add(It.Is<PromptTemplateVersion>(v => 
            v.PromptTemplateId == templateId && 
            v.Title == "Old Title" &&
            v.VersionNumber == 1)), Times.Once);
    }

    [Fact]
    public async Task RevertToVersionAsync_ShouldUpdateTemplateToVersionState_AndArchiveCurrentState()
    {
        // Arrange
        var templateId = Guid.NewGuid();
        var template = new PromptTemplate 
        { 
            Id = templateId, 
            Title = "Current Title",
            RequiredVariables = "CurrentVar"
        };
        
        var version = new PromptTemplateVersion 
        { 
            PromptTemplateId = templateId, 
            VersionNumber = 1,
            Title = "V1 Title",
            RequiredVariables = "V1Var"
        };

        var templates = new List<PromptTemplate> { template }.AsQueryable();
        var versions = new List<PromptTemplateVersion> { version }.AsQueryable();
        var versionsMock = versions.BuildMockDbSet();

        _contextMock.Setup(x => x.PromptTemplates).Returns(templates.BuildMockDbSet().Object);
        _contextMock.Setup(x => x.PromptTemplateVersions).Returns(versionsMock.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        var result = await _service.RevertToVersionAsync(templateId, 1);

        // Assert
        result.Should().BeTrue();
        template.Title.Should().Be("V1 Title");
        template.RequiredVariables.Should().Be("V1Var");
        
        // Should have archived the "Current Title" state as V2
        versionsMock.Verify(x => x.Add(It.Is<PromptTemplateVersion>(v => 
            v.PromptTemplateId == templateId && 
            v.Title == "Current Title" &&
            v.VersionNumber == 2)), Times.Once);
    }
}
