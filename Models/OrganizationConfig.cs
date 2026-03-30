namespace SmartFileOrganizer.Models;

/// <summary>
/// Configuration that controls how the organizer behaves during a run.
/// </summary>
public class OrganizationConfig
{
    /// <summary>Absolute path of the folder to organize.</summary>
    public string SourceDirectory { get; set; } = string.Empty;

    /// <summary>When true, subdirectories are also scanned.</summary>
    public bool Recursive { get; set; } = false;

    /// <summary>When true, files are not actually moved — results are only previewed.</summary>
    public bool DryRun { get; set; } = false;

    /// <summary>When true, files marked as hidden by the OS are skipped.</summary>
    public bool SkipHiddenFiles { get; set; } = true;

    /// <summary>Determines what happens when a file already exists at the destination.</summary>
    public ConflictStrategy ConflictStrategy { get; set; } = ConflictStrategy.Rename;
}

public enum ConflictStrategy
{
    /// <summary>Append a numeric suffix to make the name unique (e.g. file_1.txt).</summary>
    Rename,

    /// <summary>Leave the source file untouched and record it as skipped.</summary>
    Skip,

    /// <summary>Overwrite the existing file at the destination.</summary>
    Overwrite
}
