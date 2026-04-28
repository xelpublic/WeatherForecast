# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY Wf.Backend.sln .
COPY Wf.Application/Wf.Application.csproj Wf.Application/
COPY Wf.Domain/Wf.Domain.csproj Wf.Domain/
COPY Wf.Infrastructure/Wf.Infrastructure.csproj Wf.Infrastructure/
COPY Wf.WebApi/Wf.WebApi.csproj Wf.WebApi/

# Restore dependencies
RUN dotnet restore Wf.Backend.sln

# Copy all source code
COPY . .

# Build the application
WORKDIR /src/Wf.WebApi
RUN dotnet build -c Release --no-restore

# Publish the application
RUN dotnet publish -c Release --no-build -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Install curl for health checks (optional)
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

# Create a non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser:appuser /app
USER appuser

ENTRYPOINT ["dotnet", "Wf.WebApi.dll"]