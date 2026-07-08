using System;
using System.IO;

namespace StubGenerator;

public static class Program
{
    public static int Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.Error.WriteLine("Usage: StubGenerator <game-root> <output-dir>");
            return 1;
        }

        var gameRoot = args[0];
        var outputDir = args[1];

        if (!Directory.Exists(gameRoot))
        {
            Console.Error.WriteLine($"Game root not found: {gameRoot}");
            return 1;
        }

        Directory.CreateDirectory(outputDir);

        var assemblies = AssemblyDiscovery.Discover(gameRoot);
        Console.WriteLine($"Found {assemblies.Count} assemblies to process");

        foreach (var asm in assemblies)
            Console.WriteLine($"  {Path.GetFileName(asm)}");

        // TODO: pass to stub emitter (Task 3)

        return 0;
    }
}
