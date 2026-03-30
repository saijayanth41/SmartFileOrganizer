namespace SmartFileOrganizer.Models;

/// <summary>
/// Represents a single file discovered during a directory scan.
/// </summary>
public class FileEntry
{
    public string FullPath { get; init; }
    public string FileName { get; init; }
    public string Extension { get; init; }
    public long SizeBytes { get; init; }
    public DateTime LastModified { get; init; }

    public FileEntry(FileInfo info)
    {
        FullPath = info.FullName;
        FileName = info.Name;
        Extension = info.Extension.ToLowerInvariant();
        SizeBytes = info.Length;
        LastModified = info.LastWriteTime;
    }
}
