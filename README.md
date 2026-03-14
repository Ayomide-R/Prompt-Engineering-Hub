# Prompt Engineering Hub

Welcome to the **Prompt Engineering Hub**! This application serves as an intelligent middleman, taking vague, brief "Human-speak" task descriptions and transforming them into structured, robust "LLM-speak" prompts optimized for complex language models like GPT-4 or Gemini.

## 🌟 Features

- **Dynamic Role Selection**: Choose AI roles such as `SeniorDeveloper`, `CreativeWriter`, or `LegalExpert` to tailor the output's tone and expertise.
- **Iterative Meta-Prompt Generation**: The system evaluates your raw idea and uses a "Master Prompt" to output a highly structured formulation containing Context, Task, Constraints, and Output Formats.
- **Template Management**: Create, customize, and invoke stored Prompt Templates to eliminate repetitive typing.
- **Search & Discovery**: Find the best templates with keyword search and category filtering.
- **Prompt History with Pagination**: Efficiently browse your history with paginated results.
- **Integration with Google Gemini**: Out-of-the-box integration powered by **Microsoft Semantic Kernel**.

## 🏗️ Architecture Stack

This project is built using a highly decoupled **Clean Architecture** approach to ensure long-term maintainability.

- **Framework**: .NET 10 ASP.NET Core Web API
- **AI Integration**: Microsoft Semantic Kernel (`Microsoft.SemanticKernel.Connectors.Google`)
- **Database**: PostgreSQL via Entity Framework Core
- **Authentication**: JWT Bearer Authentication middleware
- **Input Validation**: Robust validation using **FluentValidation**
- **Global Error Handling**: Standardized RFC 7807 **ProblemDetails** responses via .NET 10 `IExceptionHandler`
- **Unit Testing**: Comprehensive test suite with over **13+ tests** using **xUnit**, **Moq**, and **FluentAssertions**
- **Docker Ready**: Production-optimized `Dockerfile` and `docker-compose.yml`.
- **API Resilience**: Built-in **Rate Limiting** policy (fixed window) to protect AI resources.
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

### Installation & Run (via Docker Compose)

The easiest way to get started is using Docker:

1. Clone the repository and set your Gemini API Key in `docker-compose.yml` or your environment variables.
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
