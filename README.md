# Prompt Engineering Hub

Welcome to the **Prompt Engineering Hub**! This application serves as an intelligent middleman, taking vague, brief "Human-speak" task descriptions and transforming them into structured, robust "LLM-speak" prompts optimized for complex language models like GPT-4 or Gemini.

## 🌟 Features

- **Multi-Model Support**: Dynamically switch between **Google Gemini**, **OpenAI GPT**, **Anthropic (Claude)**, and **Ollama (Llama)** models via the API.
- **Role-Based Access Control (RBAC)**: Fine-grained permissions with `Admin` and `User` roles.
- **Prompt Template Versioning**: Automatically archive and revert to previous template versions.
- **Integration with Microsoft Semantic Kernel**: Leverages the latest connectors for seamless AI provider orchestration.
- **Role-Based Prompt Engineering**: Configure specialized personas (e.g., `SeniorDeveloper`, `LegalExpert`) with system-wide master instructions.
- **Global Persona Management**: Admin UI to manage fallback instructions for all model roles.
- **Side-by-Side Model Comparison**: Concurrent expansion across multiple AI providers (Gemini, OpenAI, Claude, Ollama) for quality benchmarking.
- **Batch Prompt Expansion**: Bulk processing of tasks via JSON or CSV file uploads.
- **Template Management**: Create, search, and manage robust Prompt Templates.
- **Pagination & History**: Browse your generated prompt history with provider-specific metadata.

## 🏗️ Architecture Stack

This project is built using a highly decoupled **Clean Architecture** approach to ensure long-term maintainability.

- **Framework**: .NET 10 ASP.NET Core Web API
- **AI Integration**: Microsoft Semantic Kernel (`Connectors.Google`, `Connectors.OpenAI`)
- **Database**: PostgreSQL via Entity Framework Core
- **Authentication**: JWT Bearer Authentication middleware
- **Input Validation**: Robust validation using **FluentValidation**
- **Global Error Handling**: Standardized RFC 7807 **ProblemDetails** responses via .NET 10 `IExceptionHandler`
- **Unit Testing**: Comprehensive test suite with over **13+ tests** using **xUnit**, **Moq**, and **FluentAssertions**
- **Docker Ready**: Production-optimized `Dockerfile` and `docker-compose.yml`.
- **API Resilience**: Built-in **Rate Limiting** policy (fixed window) to protect AI resources.
- **Audit Logging**: Admin-only access to system audit logs for key actions.
- **Observability**: **Health Checks** for application and database readiness monitoring.

### Project Structure
- `PromptHub.Domain`: Core Entities (`User`, `PromptTemplate`, `GeneratedPrompt`) and business Rules.
- `PromptHub.Application`: Core abstractions, interfaces, Data Transfer Objects (DTOs), Validators, and Business Logic Services.
- `PromptHub.Infrastructure`: Implementation details including the EF Core `ApplicationDbContext` and Semantic Kernel integrations.
- `PromptHub.Api`: RESTful endpoints, Global Exception Handling, middleware, and Dependency Injection configurations.
- `tests/PromptHub.UnitTests`: Unit tests for core services and business logic.

## 🚀 Getting Started

### Prerequisites
- .NET 10 SDK
- Docker & Docker Compose (Recommended)
- OR Local PostgreSQL Server
- A valid Google Gemini API Key
- (Optional) A valid OpenAI API Key

### Installation & Run (via Docker Compose)

The easiest way to get started is using Docker:

1. Clone the repository and set your Gemini and/or OpenAI API Keys in `docker-compose.yml` or your environment variables.
2. Run the stack:
   ```bash
   docker-compose up --build
   ```
3. The API will be available at `http://localhost:8080` and Swagger UI at `http://localhost:8080/swagger`.

### Local Installation (Without Docker)

1. Clone the repository:
   ```bash
   git clone https://github.com/Ayomide-R/Prompt-Engineering-Hub.git
   cd Prompt-Engineering-Hub
   ```

2. Update `appsettings.json` in `src/PromptHub.Api` with your credentials.

3. Apply database migrations:
   ```bash
   dotnet tool install -g dotnet-ef
   dotnet ef database update --project src/PromptHub.Infrastructure --startup-project src/PromptHub.Api
   ```

4. Build and Run:
   ```bash
   dotnet run --project src/PromptHub.Api
   ```

### 🩺 Monitoring & Documentation

- **Health Checks**: Access `/health` to verify system readiness.
- **API Docs**: Swagger UI is enhanced with XML comments for better parameter and response descriptions.

### Running Tests
To execute the unit test suite, run:
```bash
dotnet test
```

## 🤝 Contributing
Contributions, issues, and feature requests are welcome!

## 📜 License
This project is licensed under the MIT License.
