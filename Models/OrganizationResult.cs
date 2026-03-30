namespace SmartFileOrganizer.Models;

/// <summary>
/// Collects every outcome produced by a single organization run.
/// </summary>
public class OrganizationResult
{
    public List<MovedFile> Moved { get; } = new();
    public List<SkippedFile> Skipped { get; } = new();
    public List<FailedFile> Failed { get; } = new();

    public int TotalProcessed => Moved.Count + Skipped.Count + Failed.Count;
}

/// <summary>A file that was successfully moved (or would be moved in a dry run).</summary>
public record MovedFile(string SourcePath, string DestinationPath, string Category);

/// <summary>A file that was intentionally left in place.</summary>
public record SkippedFile(string FilePath, string Reason);

/// <summary>A file that could not be moved due to an error.</summary>
public record FailedFile(string FilePath, string Error);
