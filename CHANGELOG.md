# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

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
