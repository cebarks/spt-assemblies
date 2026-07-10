using System;
using System.Diagnostics;
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

        var refasmerPath = FindRefasmer();
        if (refasmerPath == null)
        {
            Console.Error.WriteLine("refasmer not found. Install with: dotnet tool install --global JetBrains.Refasmer.CliTool");
            return 1;
        }

        var succeeded = 0;
        var failed = 0;

        foreach (var dll in assemblies)
        {
            var name = Path.GetFileName(dll);
            var psi = new ProcessStartInfo(refasmerPath, $"--all -O \"{outputDir}\" \"{dll}\"")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };

            using var proc = Process.Start(psi)!;
            proc.WaitForExit();

            if (proc.ExitCode == 0)
            {
                succeeded++;
            }
            else
            {
                var stderr = proc.StandardError.ReadToEnd().Trim();
                Console.Error.WriteLine($"  FAIL: {name}: {stderr}");
                failed++;
            }
        }

        Console.WriteLine($"\nDone: {succeeded} succeeded, {failed} failed");
        Console.WriteLine($"Reference assemblies written to {outputDir}");
        return 0;
    }

    private static string? FindRefasmer()
    {
        // Check dotnet tools path
        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var toolPath = Path.Combine(home, ".dotnet", "tools", "refasmer");
        if (File.Exists(toolPath)) return toolPath;

        // Check PATH
        try
        {
            var psi = new ProcessStartInfo("which", "refasmer")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
            };
            using var proc = Process.Start(psi)!;
            var path = proc.StandardOutput.ReadToEnd().Trim();
            proc.WaitForExit();
            if (proc.ExitCode == 0 && !string.IsNullOrEmpty(path)) return path;
        }
        catch { }

        return null;
    }
}
