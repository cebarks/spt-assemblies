# Default recipe
default:
    @just --list

# Run the reference assembly generator against a local SPT install
generate spt_path:
    mkdir -p src/SPT.ReferenceAssemblies/ref/net472
    dotnet run --project src/StubGenerator -- {{spt_path}} src/SPT.ReferenceAssemblies/ref/net472/

# Build the solution
build:
    dotnet build src/StubGenerator

# Pack the NuGet package with a specific version
pack version:
    rm -f src/SPT.ReferenceAssemblies/obj/Release/SPT.ReferenceAssemblies.{{version}}.nuspec
    dotnet pack src/SPT.ReferenceAssemblies/SPT.ReferenceAssemblies.csproj -p:Version={{version}} -o ./nupkgs

# Publish to GitHub Packages (requires GH_TOKEN env var)
publish version:
    just pack {{version}}
    dotnet nuget push ./nupkgs/SPT.ReferenceAssemblies.{{version}}.nupkg --source https://nuget.pkg.github.com/cebarks/index.json --api-key $GH_TOKEN

# Clean build artifacts.
# src/SPT.ReferenceAssemblies/ref/ is tracked source, not build output (CI packs from the
# checkout), so restore it to HEAD and drop untracked strays instead of deleting it.
clean:
    dotnet clean
    rm -rf nupkgs/
    git checkout -- src/SPT.ReferenceAssemblies/ref/
    git clean -fdx src/SPT.ReferenceAssemblies/ref/
