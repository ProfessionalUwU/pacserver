default:
    @just --list

run:
    @dotnet run

build:
    @dotnet build pacserver.csproj

publish: format
    @dotnet publish --configuration Release pacserver.csproj

format:
    @dotnet format
