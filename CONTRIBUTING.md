# Contributing to SPT.ReferenceAssemblies

## Updating for a new SPT version

1. Update your local SPT install to the new version
2. Run: `just generate ~/path/to/spt`
3. Verify the package builds: `just pack 1.0.1-sptX.Y.Z`
4. Bump `<Version>` in `src/SPT.ReferenceAssemblies/SPT.ReferenceAssemblies.csproj` to match
5. Submit a PR with the updated reference assemblies

The generated assemblies under `src/SPT.ReferenceAssemblies/ref/` are committed to the repo,
not gitignored. `publish.yml` packs from a plain checkout, so anything untracked there is
absent from the published package — and `dotnet pack` succeeds regardless.

## How it works

The generator uses [JetBrains Refasmer](https://github.com/AskDante/Refasmer) to strip method bodies from game DLLs, producing reference assemblies that contain only type signatures. This is the same approach used by [krafs/RimRef](https://github.com/krafs/RimRef) for RimWorld modding.

Assemblies already available on NuGet (BepInEx, Harmony, Newtonsoft.Json, System.*, etc.) are filtered out by `AssemblyDiscovery.cs` to avoid duplicate type conflicts.

## Modifying the skip list

If an assembly should be included or excluded, edit the skip lists in `src/StubGenerator/AssemblyDiscovery.cs`.

## Pull request checklist

- [ ] `just generate` runs without failures
- [ ] `just pack <version>` produces a valid package
- [ ] `<Version>` in `SPT.ReferenceAssemblies.csproj` matches the SPT version being shipped
- [ ] `ref/net472/` contains no `0Harmony*` or `where-allocations` assemblies
- [ ] Reference assemblies match the correct SPT version
