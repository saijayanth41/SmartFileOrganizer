using SmartFileOrganizer.Configuration;
using SmartFileOrganizer.Models;
using SmartFileOrganizer.Services.Interfaces;

namespace SmartFileOrganizer.Services;

/// <summary>
/// Core organizer: determines the destination category folder for each file,
/// handles conflicts, and performs (or simulates) the move operation.
/// </summary>
public class FileOrganizer : IFileOrganizer
{
    private readonly IConflictResolver _conflictResolver;

    public FileOrganizer(IConflictResolver conflictResolver)
    {
        _conflictResolver = conflictResolver;
    }

    public OrganizationResult Organize(IEnumerable<FileEntry> files, OrganizationConfig config)
    {
        var result = new OrganizationResult();

        foreach (var file in files)
        {
            // Skip files that live inside a category subfolder we created earlier
            // in this same run (prevents the organizer from re-processing its own output).
            if (IsInsideCategorySubfolder(file.FullPath, config.SourceDirectory))
            {
                result.Skipped.Add(new SkippedFile(file.FullPath, "Already inside a category subfolder"));
                continue;
            }

            try
            {
                var category    = CategoryMapper.GetCategory(file.Extension);
                var categoryDir = Path.Combine(config.SourceDirectory, category);
                var destination = Path.Combine(categoryDir, file.FileName);

                if (config.DryRun)
                {
                    // Preview only — show where the file would go.
                    result.Moved.Add(new MovedFile(file.FullPath, destination, category));
                    continue;
                }

                Directory.CreateDirectory(categoryDir);

                // Apply the chosen conflict strategy.
                switch (config.ConflictStrategy)
                {
                    case ConflictStrategy.Skip when File.Exists(destination):
                        result.Skipped.Add(new SkippedFile(file.FullPath, "File already exists at destination"));
                        continue;

                    case ConflictStrategy.Rename:
                        destination = _conflictResolver.Resolve(destination);
                        break;

                    // ConflictStrategy.Overwrite: fall through — File.Move with overwrite:true handles it.
                }

                File.Move(file.FullPath, destination,
                    overwrite: config.ConflictStrategy == ConflictStrategy.Overwrite);

                result.Moved.Add(new MovedFile(file.FullPath, destination, category));
            }
            catch (Exception ex)
            {
                result.Failed.Add(new FailedFile(file.FullPath, ex.Message));
            }
        }

        return result;
    }

    // Returns true when a file's directory is a direct child of the source
    // directory (i.e. it is already sitting inside one of our category folders).
    private static bool IsInsideCategorySubfolder(string filePath, string sourceDirectory)
    {
        var fileDir  = Path.GetDirectoryName(filePath);
        var parentDir = fileDir is not null ? Path.GetDirectoryName(fileDir) : null;

        return parentDir is not null &&
               string.Equals(
                   Path.GetFullPath(parentDir),
                   Path.GetFullPath(sourceDirectory),
                   StringComparison.OrdinalIgnoreCase);
    }
}
