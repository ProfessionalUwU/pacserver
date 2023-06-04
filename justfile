default:
    @just --list

project_name := `printf '%s\n' "${PWD##*/}"`
uppercase_project_name := capitalize(project_name)

setup:
    @dotnet new sln --name {{project_name}}
    @mkdir src
    @dotnet new classlib -o  src/{{uppercase_project_name}}
    @dotnet new xunit -o src/{{uppercase_project_name}}.Tests
    @dotnet sln add src/{{uppercase_project_name}}/{{uppercase_project_name}}.csproj
    @dotnet sln add src/{{uppercase_project_name}}.Tests/{{uppercase_project_name}}.Tests.csproj
    @dotnet add src/{{uppercase_project_name}}/{{uppercase_project_name}}.csproj reference src/{{uppercase_project_name}}.Tests/{{uppercase_project_name}}.Tests.csproj

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
