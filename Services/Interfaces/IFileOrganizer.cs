using SmartFileOrganizer.Models;

namespace SmartFileOrganizer.Services.Interfaces;

/// <summary>
/// Moves (or previews moving) a collection of files into category subfolders.
/// </summary>
public interface IFileOrganizer
{
    /// <summary>
    /// Processes each file in <paramref name="files"/> according to <paramref name="config"/>
    /// and returns a full summary of what was moved, skipped, or failed.
    /// </summary>
    OrganizationResult Organize(IEnumerable<FileEntry> files, OrganizationConfig config);
}
