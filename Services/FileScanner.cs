using SmartFileOrganizer.Models;
using SmartFileOrganizer.Services.Interfaces;

namespace SmartFileOrganizer.Services;

/// <summary>
/// Walks the source directory and yields a <see cref="FileEntry"/> for every
/// qualifying file. Uses deferred execution so very large directories are
/// processed without loading all paths into memory at once.
/// </summary>
public class FileScanner : IFileScanner
{
    public IEnumerable<FileEntry> Scan(OrganizationConfig config)
    {
        var searchOption = config.Recursive
            ? SearchOption.AllDirectories
            : SearchOption.TopDirectoryOnly;

        foreach (var filePath in Directory.EnumerateFiles(config.SourceDirectory, "*", searchOption))
        {
            FileInfo info;
            try
            {
                info = new FileInfo(filePath);
            }
            catch (Exception)
            {
                // Skip files we cannot stat (e.g. broken symlinks, permission denied).
                continue;
            }

            if (config.SkipHiddenFiles && (info.Attributes & FileAttributes.Hidden) != 0)
                continue;

            yield return new FileEntry(info);
        }
    }
}
