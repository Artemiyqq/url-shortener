# Use the ASP.NET runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7047
EXPOSE 5095

# Build and publish the Razor app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["UrlShortener.Algorithm/UrlShortener.Algorithm.csproj", "UrlShortener.Algorithm/"]
RUN dotnet restore "UrlShortener.Algorithm/UrlShortener.Algorithm.csproj"
COPY . .
WORKDIR "/src/UrlShortener.Algorithm"
RUN dotnet publish -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UrlShortener.Algorithm.dll"]
