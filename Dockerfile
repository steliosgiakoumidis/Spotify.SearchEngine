FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY ./ .

RUN dotnet build src/Spotify.SearchEngine.Api/Spotify.SearchEngine.Api.csproj

RUN dotnet test tests/Spotify.SearchEngine.Api.Tests/Spotify.SearchEngine.Api.Tests.csproj
RUN dotnet test tests/Spotify.SearchEngine.Application.Tests/Spotify.SearchEngine.Application.Tests.csproj

RUN dotnet publish src/Spotify.SearchEngine.Api/Spotify.SearchEngine.Api.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Spotify.SearchEngine.Api.dll"]