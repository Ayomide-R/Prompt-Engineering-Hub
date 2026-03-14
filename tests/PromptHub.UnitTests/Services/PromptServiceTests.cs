using FluentAssertions;
using Moq;
using PromptHub.Application.Interfaces;
using PromptHub.Application.Services;
using PromptHub.Domain.Entities;
using PromptHub.Domain.Enums;
using PromptHub.UnitTests.Helpers;
using Xunit;

namespace PromptHub.UnitTests.Services;

public class PromptServiceTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly Mock<IAiProviderService> _aiProviderMock;
    private readonly PromptService _promptService;

    public PromptServiceTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _aiProviderMock = new Mock<IAiProviderService>();
        _promptService = new PromptService(_contextMock.Object, _aiProviderMock.Object);
    }

    [Fact]
    public async Task ExpandPromptAsync_ShouldReturnGeneratedPrompt_WithoutTemplate()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var originalInput = "Create a hello world app";
        var expandedText = "System: Expert... User: Create a hello world app";
        
        var prompts = new List<GeneratedPrompt>().AsQueryable();
        var mockDbSet = prompts.BuildMockDbSet();
        _contextMock.Setup(c => c.GeneratedPrompts).Returns(mockDbSet.Object);
        
        _aiProviderMock.Setup(a => a.GenerateExpandedPromptAsync(originalInput, RoleType.GeneralAssistant, It.IsAny<string>()))
            .ReturnsAsync(expandedText);

        // Act
        var result = await _promptService.ExpandPromptAsync(originalInput, null, userId);

        // Assert
        result.Should().NotBeNull();
        result.FinalPrompt.Should().Be(expandedText);
        result.UserId.Should().Be(userId);
        _contextMock.Verify(c => c.GeneratedPrompts.Add(It.IsAny<GeneratedPrompt>()), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task ExpandPromptAsync_ShouldUseTemplate_WhenProvided()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var templateId = Guid.NewGuid();
        var template = new PromptTemplate 
        { 
            Id = templateId, 
            DefaultRole = RoleType.CreativeWriter, 
            MasterInstruction = "Write creatively" 
        };
        
        var templates = new List<PromptTemplate> { template }.AsQueryable();
        var mockTemplateDbSet = templates.BuildMockDbSet();
        _contextMock.Setup(c => c.PromptTemplates).Returns(mockTemplateDbSet.Object);
        
        // FindAsync is harder to mock with IQueryable helper, so we setup directly
        _contextMock.Setup(c => c.PromptTemplates.FindAsync(templateId)).ReturnsAsync(template);

        var prompts = new List<GeneratedPrompt>().AsQueryable();
        var mockPromptDbSet = prompts.BuildMockDbSet();
        _contextMock.Setup(c => c.GeneratedPrompts).Returns(mockPromptDbSet.Object);

        _aiProviderMock.Setup(a => a.GenerateExpandedPromptAsync(It.IsAny<string>(), RoleType.CreativeWriter, "Write creatively"))
            .ReturnsAsync("Creative result");

        // Act
        var result = await _promptService.ExpandPromptAsync("Input", templateId, userId);

        // Assert
        result.UsedRole.Should().Be(RoleType.CreativeWriter);
        result.FinalPrompt.Should().Be("Creative result");
    }
}
