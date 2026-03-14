FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files and restore
COPY ["src/PromptHub.Api/PromptHub.Api.csproj", "src/PromptHub.Api/"]
COPY ["src/PromptHub.Application/PromptHub.Application.csproj", "src/PromptHub.Application/"]
COPY ["src/PromptHub.Domain/PromptHub.Domain.csproj", "src/PromptHub.Domain/"]
COPY ["src/PromptHub.Infrastructure/PromptHub.Infrastructure.csproj", "src/PromptHub.Infrastructure/"]
RUN dotnet restore "src/PromptHub.Api/PromptHub.Api.csproj"

# Build and publish
COPY . .
WORKDIR "/src/src/PromptHub.Api"
RUN dotnet publish "PromptHub.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PromptHub.Api.dll"]
