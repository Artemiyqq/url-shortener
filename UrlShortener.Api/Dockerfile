# Use the .NET SDK image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-amd64 AS base
WORKDIR /app
EXPOSE 7006

# Build and publish the app
FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy-amd64 AS build
WORKDIR /src
COPY ["UrlShortener.Api/UrlShortener.Api.csproj", "UrlShortener.Api/"]
RUN dotnet restore "UrlShortener.Api/UrlShortener.Api.csproj"
COPY . .
WORKDIR "/src/UrlShortener.Api"
RUN dotnet publish -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UrlShortener.Api.dll"]
