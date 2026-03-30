using SmartFileOrganizer.Models;

namespace SmartFileOrganizer.Services.Interfaces;

/// <summary>
/// Scans a directory and returns the files that should be organized.
/// </summary>
public interface IFileScanner
{
    /// <summary>
    /// Enumerates files in the source directory described by <paramref name="config"/>.
    /// Results are streamed — callers should not assume they are fully buffered.
    /// </summary>
    IEnumerable<FileEntry> Scan(OrganizationConfig config);
}
