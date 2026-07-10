# SPT.ReferenceAssemblies

Reference assemblies for building [SPT](https://sp-tarkov.com/) BepInEx plugins without needing the game installed.

## What this is

A NuGet package containing reference assemblies (method bodies stripped) for:

- **Unity Engine** modules (MonoBehaviour, GUI/IMGUI, Physics, etc.)
- **EFT** types from Assembly-CSharp (EFT.Builds, EFT.InventoryLogic, EFT.UI, etc.)
- **SPT** module types (SPT.Reflection.Patching, SPT.Common.Http, etc.)
- Other game assemblies (Comfort, ItemComponent.Types, Sirenix, etc.)

BepInEx and Harmony are already on NuGet — this package fills the gap for everything else.

Reference assemblies contain only type signatures with no implementation code. They're produced using [JetBrains Refasmer](https://github.com/AskDante/Refasmer), inspired by [krafs/RimRef](https://github.com/krafs/RimRef) from the RimWorld modding community.

## Why it exists

SPT mod projects typically reference game DLLs via `<HintPath>` pointing at a local game install. This means:

- CI builds are impossible without the game installed on the build server
- Every contributor needs to configure local paths
- Projects must live inside the game directory structure

This package replaces all those HintPath references with a single NuGet dependency.

## Usage

Replace your `<Reference>` / `<HintPath>` blocks with:

```xml
<PackageReference Include="SPT.ReferenceAssemblies" Version="1.0.0-spt4.0.13" PrivateAssets="all" />
```

`PrivateAssets="all"` ensures the reference assemblies are compile-only — at runtime, the real DLLs are loaded by the game.

You'll still need BepInEx as a separate NuGet reference:

```xml
<PackageReference Include="BepInEx.Core" Version="5.*" />
```

Add the BepInEx NuGet feed to your `nuget.config`:

```xml
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <add key="BepInEx" value="https://nuget.bepinex.dev/v3/index.json" />
  </packageSources>
</configuration>
```

## Versioning

Package versions use a pre-release suffix to indicate the SPT version:

- `1.0.0-spt4.0.13` — reference assemblies from SPT 4.0.13

## Regenerating reference assemblies

If you need to update for a new SPT version:

1. Install [just](https://just.systems/) and [Refasmer](https://github.com/AskDante/Refasmer): `dotnet tool install --global JetBrains.Refasmer.CliTool`
2. Run: `just generate ~/path/to/spt-install`
3. Review the diff, commit, tag, push

## License

LGPL-3.0-only
