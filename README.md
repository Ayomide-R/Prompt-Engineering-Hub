# Prompt Engineering Hub

Welcome to the **Prompt Engineering Hub**! This application serves as an intelligent middleman, taking vague, brief "Human-speak" task descriptions and transforming them into structured, robust "LLM-speak" prompts optimized for complex language models like GPT-4 or Gemini.

## 🌟 Features

- **Dynamic Role Selection**: Choose AI roles such as `SeniorDeveloper`, `CreativeWriter`, or `LegalExpert` to tailor the output's tone and expertise.
- **Iterative Meta-Prompt Generation**: The system evaluates your raw idea and uses a "Master Prompt" to output a highly structured formulation containing Context, Task, Constraints, and Output Formats.
- **Template Management**: Create, customize, and invoke stored Prompt Templates to eliminate repetitive typing.
- **Integration with Google Gemini**: Out-of-the-box integration powered by **Microsoft Semantic Kernel**.

## 🏗️ Architecture Stack

This project is built using a highly decoupled **Clean Architecture** approach to ensure long-term maintainability.

- **Framework**: .NET 10 ASP.NET Core Web API
- **AI Integration**: Microsoft Semantic Kernel (`Microsoft.SemanticKernel.Connectors.Google`)
- **Database**: PostgreSQL via Entity Framework Core
- **Authentication**: JWT (Planned)

### Project Structure
- `PromptHub.Domain`: Core Entities (`User`, `PromptTemplate`, `GeneratedPrompt`) and business Rules.
- `PromptHub.Application`: Core abstractions, interfaces (`IApplicationDbContext`), and Business Logic Services (`PromptService`).
- `PromptHub.Infrastructure`: Implementation details including the EF Core `ApplicationDbContext` and Semantic Kernel integrations.
- `PromptHub.Api`: RESTful endpoints, middleware, and Dependency Injection configurations.

## 🚀 Getting Started

### Prerequisites
- .NET 10 SDK
- PostgreSQL Server
- A valid Google Gemini API Key

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/Ayomide-R/Prompt-Engineering-Hub.git
   cd Prompt-Engineering-Hub
   ```

2. Update `appsettings.json` in `src/PromptHub.Api` with your credentials:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=PromptHubDb;Username=postgres;Password=your_password"
     },
     "AIProvider": {
       "Gemini": {
         "ApiKey": "YOUR_ACTUAL_GEMINI_API_KEY",
         "ModelId": "gemini-2.5-flash"
       }
     }
   }
   ```

3. Build and Run the project:
   ```bash
   dotnet build
   cd src/PromptHub.Api
   dotnet run
   ```

4. Navigate to the local Swagger UI to safely test your endpoints:
   `http://localhost:<port>/swagger`

## 🤝 Contributing
Contributions, issues, and feature requests are welcome!

## 📜 License
This project is licensed under the MIT License.
