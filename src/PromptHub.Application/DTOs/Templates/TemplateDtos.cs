using PromptHub.Domain.Enums;

namespace PromptHub.Application.DTOs.Templates;

public record CreateTemplateRequest(
    string Title, 
    string Description, 
    string Category, 
    RoleType DefaultRole, 
    string MasterInstruction, 
    string RequiredVariables, 
    bool IsPublic);

public record UpdateTemplateRequest(
    string Title, 
    string Description, 
    string Category, 
    RoleType DefaultRole, 
    string MasterInstruction, 
    string RequiredVariables, 
    bool IsPublic);

public record TemplateResponse(
    Guid Id, 
    string Title, 
    string Description, 
    string Category, 
    RoleType DefaultRole, 
    string MasterInstruction, 
    string RequiredVariables, 
    bool IsPublic, 
    DateTime CreatedAt, 
    Guid? TemplateId);

public record TemplateVersionResponse(
    Guid Id,
    int VersionNumber,
    string Title,
    string Description,
    string Category,
    RoleType DefaultRole,
    string MasterInstruction,
    string RequiredVariables,
    DateTime CreatedAt);
