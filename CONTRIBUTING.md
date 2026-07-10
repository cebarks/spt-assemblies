# Contributing to SPT.ReferenceAssemblies

## Updating for a new SPT version

1. Update your local SPT install to the new version
2. Run: `just generate ~/path/to/spt`
3. Verify the package builds: `just pack 1.0.0-sptX.Y.Z`
4. Submit a PR with the updated reference assemblies

## How it works

The generator uses [JetBrains Refasmer](https://github.com/AskDante/Refasmer) to strip method bodies from game DLLs, producing reference assemblies that contain only type signatures. This is the same approach used by [krafs/RimRef](https://github.com/krafs/RimRef) for RimWorld modding.

Assemblies already available on NuGet (BepInEx, Harmony, Newtonsoft.Json, System.*, etc.) are filtered out by `AssemblyDiscovery.cs` to avoid duplicate type conflicts.

## Modifying the skip list

If an assembly should be included or excluded, edit the skip lists in `src/StubGenerator/AssemblyDiscovery.cs`.

## Pull request checklist

- [ ] `just generate` runs without failures
- [ ] `just pack <version>` produces a valid package
- [ ] Reference assemblies match the correct SPT version
