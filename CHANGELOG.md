# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- **Phase 8: High-Performance React Migration**:
  - **Framework Transition**: Migrated the entire frontend from Blazor WebAssembly to **React 18+** with **Vite** and **TypeScript** for superior performance and developer experience.
  - **Premium Animation Suite**: Integrated **Framer Motion** for fluid entrance animations, staggered list reveals, and smooth layout transitions.
  - **Modern Iconography**: Implemented **Lucide React** for a clean, consistent, and professional icon set.
  - **Refined Authentication Flow**: 
    - Finalized the **User Registration** page with a premium animated UI.
    - Added "Confirm Password" validation and enhanced error handling.
    - Seamless automatic login and redirection to the workspace upon onboarding.
  - **Enhanced Workspace UI**: Improved the prompt expansion editor with better state management and responsive design.
- **Phase 7: Model Comparison & Batch Expansion**:
  - **Global Persona Management**: System-wide registry for role-based master instructions with specialized Admin UI.
  - **Side-by-Side Comparison**: New dual-card UI to compare prompt expansion across multiple AI models concurrently.
  - **Batch Expansion**: Support for bulk-processing tasks via CSV/JSON file uploads in the frontend.
  - **Provider Tracking**: Added `UsedProvider` to `GeneratedPrompt` domain and API for improved traceability.
  - **Admin Navigation**: Updated NavMenu to include Personas, Compare, and Batch tools.

### Changed
- **Frontend Stack**: Migrated from Blazor WebAssembly to **React 18+** for improved performance, animation capabilities (Framer Motion), and modern developer tooling (Vite).
- **Authentication Handlers**: Replaced Blazor-specific JWT handlers with a React Context-based authentication provider.

### Removed
- **Blazor WebAssembly Project**: Deprecated and replaced the `PromptHub.Web` (Blazor) project with `PromptHub.React`.
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
