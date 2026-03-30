namespace SmartFileOrganizer.Services.Interfaces;

/// <summary>
/// Resolves naming conflicts when a file already exists at a target path.
/// </summary>
public interface IConflictResolver
{
    /// <summary>
    /// Given a desired destination path that may already be occupied,
    /// returns a safe path guaranteed not to collide with an existing file.
    /// </summary>
    string Resolve(string destinationPath);
}
