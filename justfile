default:
    @just --list

run:
    @dotnet run

build:
    @dotnet build src/Pacserver/pacserver.csproj
    @dotnet build src/Pacserver.Tests/Pacserver.Tests.csproj

publish: format
    @dotnet publish --configuration Release src/Pacserver/pacserver.csproj

format:
    @dotnet format src/Pacserver
    @dotnet format src/Pacserver.Tests

test: build
    @dotnet test src/Pacserver.Tests
