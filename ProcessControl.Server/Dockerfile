# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the entire solution directory
COPY . .

# Build and publish the application
WORKDIR /src/ProcessControl.Server

# Restore dependencies
RUN dotnet restore ProcessControl.Server.sln
RUN dotnet publish "ProcessControl.API.csproj" -c Release -o /app/publish

# Stage 2: Final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProcessControl.API.dll"]
