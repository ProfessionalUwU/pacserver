default:
    @just --list

run:
    @dotnet run

build:
    @dotnet build pacserver.csproj

publish:
    @dotnet publish --configuration Release pacserver.csproj
