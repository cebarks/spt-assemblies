# Default recipe
default:
    @just --list

# Run the reference assembly generator against a local SPT install
generate spt_path:
    mkdir -p src/SPT.ReferenceAssemblies/ref/net472
    dotnet run --project src/StubGenerator -- {{spt_path}} src/SPT.ReferenceAssemblies/ref/net472/

# Build the solution
build:
    dotnet build

# Pack the NuGet package with a specific version
pack version:
    dotnet pack src/SPT.ReferenceAssemblies/SPT.ReferenceAssemblies.csproj -p:Version={{version}} -o ./nupkgs

# Publish to NuGet.org (requires NUGET_API_KEY env var)
publish version:
    just pack {{version}}
    dotnet nuget push ./nupkgs/SPT.ReferenceAssemblies.{{version}}.nupkg --source https://api.nuget.org/v3/index.json --api-key $NUGET_API_KEY

# Clean build artifacts
clean:
    dotnet clean
    rm -rf nupkgs/ src/SPT.ReferenceAssemblies/ref/
