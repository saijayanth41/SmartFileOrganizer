using SmartFileOrganizer.Models;
using SmartFileOrganizer.Services;
using SmartFileOrganizer.Services.Interfaces;

namespace SmartFileOrganizer;

class Program
{
    static int Main(string[] args)
    {
        
        Console.WriteLine("    Smart File Organizer     ");
        Console.WriteLine();

        var config = ParseArguments(args);
        if (config is null)
        {
            PrintUsage();
            return 1;
        }

        if (!Directory.Exists(config.SourceDirectory))
        {
            Console.WriteLine($"Error: Directory not found: {config.SourceDirectory}");
            return 1;
        }

        if (config.DryRun)
        {
            Console.WriteLine("[ DRY RUN ] No files will be moved.\n");
        }

        IFileScanner    scanner          = new FileScanner();
        IConflictResolver conflictResolver = new ConflictResolver();
        IFileOrganizer  organizer        = new FileOrganizer(conflictResolver);

        Console.WriteLine($"Source : {Path.GetFullPath(config.SourceDirectory)}");
        Console.WriteLine($"Mode   : {(config.DryRun ? "Preview" : "Organize")} | " +
                          $"Recursive: {config.Recursive} | " +
                          $"Conflicts: {config.ConflictStrategy}");
        Console.WriteLine();

        Console.Write("Scanning... ");
        var files = scanner.Scan(config).ToList();
        Console.WriteLine($"{files.Count} file(s) found.\n");

        if (files.Count == 0)
        {
            Console.WriteLine("Nothing to organize. The folder is already clean.");
            return 0;
        }

        var result = organizer.Organize(files, config);
        PrintResult(result, config.DryRun);
        return result.Failed.Count > 0 ? 2 : 0;
    }

    // ── Argument Parsing ──────────────────────────────────────────────────────

    static OrganizationConfig? ParseArguments(string[] args)
    {
        if (args.Length == 0)
            return null;

        var config = new OrganizationConfig
        {
            SourceDirectory = args[0]
        };

        for (int i = 1; i < args.Length; i++)
        {
            switch (args[i].ToLowerInvariant())
            {
                case "--dry-run":
                    config.DryRun = true;
                    break;

                case "--recursive":
                    config.Recursive = true;
                    break;

                case "--include-hidden":
                    config.SkipHiddenFiles = false;
                    break;

                case "--conflict":
                    if (i + 1 < args.Length &&
                        Enum.TryParse<ConflictStrategy>(args[++i], ignoreCase: true, out var strategy))
                    {
                        config.ConflictStrategy = strategy;
                    }
                    else
                    {
                        Console.WriteLine($"Warning: Unknown conflict strategy '{(i < args.Length ? args[i] : "")}'. Using default (Rename).");
                    }
                    break;

                default:
                    Console.WriteLine($"Warning: Unknown option '{args[i]}' — ignored.");
                    break;
            }
        }

        return config;
    }

    // ── Output ────────────────────────────────────────────────────────────────

    static void PrintResult(OrganizationResult result, bool dryRun)
    {
        var verb = dryRun ? "Would move" : "Moved";

        if (result.Moved.Count > 0)
        {
            Console.WriteLine("── Organized files ──────────────────────────────");
            var byCategory = result.Moved
                .GroupBy(m => m.Category)
                .OrderBy(g => g.Key);

            foreach (var group in byCategory)
            {
                Console.WriteLine($"\n  [{group.Key}]  ({group.Count()} file(s))");
                foreach (var moved in group)
                {
                    Console.WriteLine($"    {verb}: {Path.GetFileName(moved.SourcePath)}");
                    Console.WriteLine($"       → {moved.DestinationPath}");
                }
            }
            Console.WriteLine();
        }

        if (result.Skipped.Count > 0)
        {
            Console.WriteLine("── Skipped ──────────────────────────────────────");
            foreach (var skipped in result.Skipped)
                Console.WriteLine($"  {Path.GetFileName(skipped.FilePath)}: {skipped.Reason}");
            Console.WriteLine();
        }

        if (result.Failed.Count > 0)
        {
            Console.WriteLine("── Errors ───────────────────────────────────────");
            foreach (var failed in result.Failed)
                Console.WriteLine($"  {Path.GetFileName(failed.FilePath)}: {failed.Error}");
            Console.WriteLine();
        }

        Console.WriteLine("─────────────────────────────────────────────────");
        Console.WriteLine($"Summary:  {result.Moved.Count} moved  |  " +
                          $"{result.Skipped.Count} skipped  |  " +
                          $"{result.Failed.Count} failed  " +
                          $"(total processed: {result.TotalProcessed})");
    }

    static void PrintUsage()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("  SmartFileOrganizer <directory> [options]");
        Console.WriteLine();
        Console.WriteLine("Arguments:");
        Console.WriteLine("  <directory>          Path of the folder to organize");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  --dry-run            Preview changes without moving any files");
        Console.WriteLine("  --recursive          Also organize files inside subdirectories");
        Console.WriteLine("  --include-hidden     Include hidden files (skipped by default)");
        Console.WriteLine("  --conflict <mode>    How to handle name collisions:");
        Console.WriteLine("                         Rename   — append _1, _2 … (default)");
        Console.WriteLine("                         Skip     — leave the source file in place");
        Console.WriteLine("                         Overwrite— replace the existing file");
        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine("  SmartFileOrganizer ~/Downloads");
        Console.WriteLine("  SmartFileOrganizer ~/Downloads --dry-run");
        Console.WriteLine("  SmartFileOrganizer ~/Downloads --recursive --conflict Skip");
        Console.WriteLine("  SmartFileOrganizer C:\\Users\\You\\Desktop --conflict Overwrite");
    }
}
