using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StubGenerator;

public static class AssemblyDiscovery
{
    private static readonly HashSet<string> SkipAssemblies = new(StringComparer.OrdinalIgnoreCase)
    {
        "Newtonsoft.Json",
        "0Harmony",
        "BepInEx",
        "BepInEx.Preloader",
        "BepInEx.Harmony",
        "System",
        "mscorlib",
        "netstandard",
        "websocket-sharp",
        "LiteDB",
        "ICSharpCode.SharpZipLib",
    };

    private static readonly string[] SkipPrefixes =
    {
        "System.",
        "Microsoft.",
        "Mono.",
        "MonoMod.",
    };

    public static IReadOnlyList<string> Discover(string gameRoot)
    {
        var dirs = new[]
        {
            Path.Combine(gameRoot, "EscapeFromTarkov_Data", "Managed"),
            Path.Combine(gameRoot, "BepInEx", "plugins", "spt"),
            Path.Combine(gameRoot, "BepInEx", "core"),
        };

        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var result = new List<string>();

        foreach (var dir in dirs)
        {
            if (!Directory.Exists(dir))
            {
                Console.Error.WriteLine($"Warning: directory not found: {dir}");
                continue;
            }

            foreach (var dll in Directory.EnumerateFiles(dir, "*.dll"))
            {
                var name = Path.GetFileNameWithoutExtension(dll);

                if (SkipAssemblies.Contains(name))
                    continue;

                if (SkipPrefixes.Any(p => name.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
                    continue;

                if (!seen.Add(name))
                    continue;

                result.Add(dll);
            }
        }

        return result;
    }
}
