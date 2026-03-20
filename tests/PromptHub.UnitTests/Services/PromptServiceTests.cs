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
    private readonly Mock<IPersonaService> _personaServiceMock;
    private readonly PromptService _promptService;

    public PromptServiceTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _aiProviderMock = new Mock<IAiProviderService>();
        _personaServiceMock = new Mock<IPersonaService>();
        _promptService = new PromptService(_contextMock.Object, _aiProviderMock.Object, _personaServiceMock.Object);
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
        
        _aiProviderMock.Setup(a => a.GenerateExpandedPromptAsync(originalInput, RoleType.GeneralAssistant, It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<Dictionary<string, string>?>()))
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

        _aiProviderMock.Setup(a => a.GenerateExpandedPromptAsync(It.IsAny<string>(), RoleType.CreativeWriter, "Write creatively", It.IsAny<string?>(), It.IsAny<Dictionary<string, string>?>()))
            .ReturnsAsync("Creative result");

        // Act
        var result = await _promptService.ExpandPromptAsync("Input", templateId, userId);

        // Assert
        result.UsedRole.Should().Be(RoleType.CreativeWriter);
        result.FinalPrompt.Should().Be("Creative result");
    }

    [Fact]
    public async Task GetUserPromptsAsync_ShouldReturnPaginatedResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var prompts = new List<GeneratedPrompt>
        {
            new GeneratedPrompt { Id = Guid.NewGuid(), UserId = userId, GeneratedAt = DateTime.UtcNow.AddMinutes(-10) },
            new GeneratedPrompt { Id = Guid.NewGuid(), UserId = userId, GeneratedAt = DateTime.UtcNow.AddMinutes(-5) },
            new GeneratedPrompt { Id = Guid.NewGuid(), UserId = userId, GeneratedAt = DateTime.UtcNow }
        }.AsQueryable();

        _contextMock.Setup(c => c.GeneratedPrompts).Returns(prompts.BuildMockDbSet().Object);

        // Act
        var result = await _promptService.GetUserPromptsAsync(userId, pageNumber: 1, pageSize: 2);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(3);
        result.TotalPages.Should().Be(2);
        result.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public async Task ExpandPromptMultiAsync_ShouldReturnMultiResults()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var providers = new List<string> { "Gemini", "OpenAI" };
        
        _aiProviderMock.Setup(a => a.GenerateExpandedPromptAsync(It.IsAny<string>(), It.IsAny<RoleType>(), It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<Dictionary<string, string>?>()))
            .ReturnsAsync("Result");

        var prompts = new List<GeneratedPrompt>().AsQueryable();
        _contextMock.Setup(c => c.GeneratedPrompts).Returns(prompts.BuildMockDbSet().Object);

        // Act
        var results = await _promptService.ExpandPromptMultiAsync("Task", null, userId, providers);

        // Assert
        results.Should().HaveCount(2);
        _aiProviderMock.Verify(a => a.GenerateExpandedPromptAsync(It.IsAny<string>(), It.IsAny<RoleType>(), It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<Dictionary<string, string>?>()), Times.Exactly(2));
    }

    [Fact]
    public async Task ExpandPromptBatchAsync_ShouldReturnBatchResults()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var inputs = new List<string> { "Task 1", "Task 2", "Task 3" };
        
        _aiProviderMock.Setup(a => a.GenerateExpandedPromptAsync(It.IsAny<string>(), It.IsAny<RoleType>(), It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<Dictionary<string, string>?>()))
            .ReturnsAsync("Result");

        var prompts = new List<GeneratedPrompt>().AsQueryable();
        _contextMock.Setup(c => c.GeneratedPrompts).Returns(prompts.BuildMockDbSet().Object);

        // Act
        var results = await _promptService.ExpandPromptBatchAsync(inputs, null, userId);

        // Assert
        results.Should().HaveCount(3);
        _aiProviderMock.Verify(a => a.GenerateExpandedPromptAsync(It.IsAny<string>(), It.IsAny<RoleType>(), It.IsAny<string>(), It.IsAny<string?>(), It.IsAny<Dictionary<string, string>?>()), Times.Exactly(3));
    }
}
