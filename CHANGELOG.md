# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- **Multi-Model Support Completion**: Integrated **Anthropic (Claude 3)** and **Ollama (Local Llama)** support alongside Google Gemini and OpenAI.
- **Role-Based Access Control (RBAC)**: Introduced `Admin` and `User` roles with policy-based authorization.
- **Template Versioning**: Implemented automatic version archiving on update and revert capabilities.
- **Audit Logs**: Added admin-only `AuditController` to monitor system activities.
- **Dynamic Provider Selection**: Users can now specify `"Gemini"`, `"OpenAI"`, `"Anthropic"`, or `"Ollama"` in expansion requests.
- Implemented **Entity Framework Core Migrations** for schema management.
- Added **Pagination** for User Prompt History (TotalCount, TotalPages, PageNumber).
- Added **Search & Filter** capabilities for Public Templates (filter by keyword and category).
- Added **Update** and **Delete** endpoints for Prompt Templates with ownership validation.
- Enhanced Audit Logging for template modification and deletion.
- Implemented **Structured Logging** with Serilog, including request logging and audit trails.
- Updated documentation with database migration instructions.
- Implemented JWT Bearer Authentication middleware and secured endpoints.
- Added `AuthController` for user registration and login endpoints.
- Created Auth-related Request and Response Data Transfer Objects (DTOs).
- Introduced DTOs for `PromptTemplate` and `GeneratedPrompt` to decouple the API from the Domain layer.
- Integrated `FluentValidation` for robust input validation across all API endpoints.
- Secured `PromptController` with `[Authorize]` and removed placeholder GUIDs.
- Implemented a Global Exception Handler using the .NET 10 `IExceptionHandler` interface for standardized `ProblemDetails` responses.
- Refactored controllers to remove redundant try-catch blocks, relying on the global handler.
- Setup a comprehensive testing infrastructure using `xUnit`, `Moq`, and `FluentAssertions`.
- Implemented unit tests for `AuthService` and `PromptService` with a custom `MockDbSet` helper for EF Core 10.
- Integrated **Serilog** for structured logging and auditing.
- Configured console and rolling file sinks with JSON formatting for logs.
- Added audit logging to `AuthController`, `TemplateController`, and `PromptController`.
- Transitioned from `EnsureCreated()` to **EF Core Migrations** for reliable schema versioning.
- Automated migration application on application startup.
- Integrated `Microsoft.EntityFrameworkCore.Design` for developer tooling support.
- Synchronized **Entity Framework Core 10.0.5** and related packages across all projects to resolve version conflicts.
- Cleaned up build warnings, achieving a **0 Warning, 0 Error** build status.

### Changed
- Configured PostgreSQL database connection string in `appsettings.json`.

## [0.1.0] - 2026-03-11

### Added
- Initialized ASP.NET Core Web API Solution (.NET 10) tracking Clean Architecture principles.
- Created `PromptHub.Domain` containing entities `User`, `PromptTemplate`, and `GeneratedPrompt`.
- Created `PromptHub.Application` defining internal `PromptService` and `PromptTemplateService` abstractions.
- Created `PromptHub.Infrastructure` configuring EF Core for PostgreSQL.
- Implemented Google Gemini integration utilizing Microsoft Semantic Kernel (`GeminiAiProviderService`).
- Exposed RESTful API Endpoints (`PromptController`, `TemplateController`) for expanding prompts, managing templates, and accessing history.
- Added Swagger generation for localized API testing.
- Initialized README documentation and Changelog.
