using SmartFileOrganizer.Services.Interfaces;

namespace SmartFileOrganizer.Services;

/// <summary>
/// Resolves naming conflicts by appending an incrementing counter suffix.
/// Example: "report.pdf" → "report_1.pdf" → "report_2.pdf" …
/// </summary>
public class ConflictResolver : IConflictResolver
{
    public string Resolve(string destinationPath)
    {
        if (!File.Exists(destinationPath))
            return destinationPath;

        var directory = Path.GetDirectoryName(destinationPath)!;
        var stem      = Path.GetFileNameWithoutExtension(destinationPath);
        var ext       = Path.GetExtension(destinationPath);

        int counter = 1;
        string candidate;
        do
        {
            candidate = Path.Combine(directory, $"{stem}_{counter}{ext}");
            counter++;
        }
        while (File.Exists(candidate));

        return candidate;
    }
}
