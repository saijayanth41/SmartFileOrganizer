namespace SmartFileOrganizer.Configuration;

/// <summary>
/// Maps file extensions to human-readable category folder names.
/// Add or modify entries here to customise how files are grouped.
/// </summary>
public static class CategoryMapper
{
    private static readonly Dictionary<string, string> ExtensionToCategory =
        new(StringComparer.OrdinalIgnoreCase)
        {
            // ── Images ───────────────────────────────────────────────────────────
            [".jpg"]  = "Images",
            [".jpeg"] = "Images",
            [".png"]  = "Images",
            [".gif"]  = "Images",
            [".bmp"]  = "Images",
            [".svg"]  = "Images",
            [".webp"] = "Images",
            [".ico"]  = "Images",
            [".tiff"] = "Images",
            [".tif"]  = "Images",
            [".heic"] = "Images",
            [".raw"]  = "Images",

            // ── Documents ────────────────────────────────────────────────────────
            [".pdf"]  = "Documents",
            [".doc"]  = "Documents",
            [".docx"] = "Documents",
            [".txt"]  = "Documents",
            [".rtf"]  = "Documents",
            [".odt"]  = "Documents",
            [".pages"]= "Documents",
            [".md"]   = "Documents",
            [".epub"] = "Documents",

            // ── Spreadsheets ─────────────────────────────────────────────────────
            [".xls"]    = "Spreadsheets",
            [".xlsx"]   = "Spreadsheets",
            [".csv"]    = "Spreadsheets",
            [".ods"]    = "Spreadsheets",
            [".numbers"]= "Spreadsheets",

            // ── Presentations ────────────────────────────────────────────────────
            [".ppt"]  = "Presentations",
            [".pptx"] = "Presentations",
            [".odp"]  = "Presentations",
            [".key"]  = "Presentations",

            // ── Videos ───────────────────────────────────────────────────────────
            [".mp4"]  = "Videos",
            [".mkv"]  = "Videos",
            [".avi"]  = "Videos",
            [".mov"]  = "Videos",
            [".wmv"]  = "Videos",
            [".flv"]  = "Videos",
            [".webm"] = "Videos",
            [".m4v"]  = "Videos",
            [".mpg"]  = "Videos",
            [".mpeg"] = "Videos",

            // ── Audio ─────────────────────────────────────────────────────────────
            [".mp3"]  = "Audio",
            [".wav"]  = "Audio",
            [".flac"] = "Audio",
            [".aac"]  = "Audio",
            [".ogg"]  = "Audio",
            [".wma"]  = "Audio",
            [".m4a"]  = "Audio",
            [".aiff"] = "Audio",

            // ── Archives ──────────────────────────────────────────────────────────
            [".zip"] = "Archives",
            [".rar"] = "Archives",
            [".7z"]  = "Archives",
            [".tar"] = "Archives",
            [".gz"]  = "Archives",
            [".bz2"] = "Archives",
            [".xz"]  = "Archives",

            // ── Code ──────────────────────────────────────────────────────────────
            [".cs"]   = "Code",
            [".py"]   = "Code",
            [".js"]   = "Code",
            [".ts"]   = "Code",
            [".html"] = "Code",
            [".css"]  = "Code",
            [".java"] = "Code",
            [".cpp"]  = "Code",
            [".c"]    = "Code",
            [".h"]    = "Code",
            [".go"]   = "Code",
            [".rs"]   = "Code",
            [".rb"]   = "Code",
            [".php"]  = "Code",
            [".swift"]= "Code",
            [".kt"]   = "Code",
            [".sh"]   = "Code",
            [".ps1"]  = "Code",
            [".sql"]  = "Code",
            [".xml"]  = "Code",
            [".json"] = "Code",
            [".yaml"] = "Code",
            [".yml"]  = "Code",
            [".toml"] = "Code",

            // ── Executables ───────────────────────────────────────────────────────
            [".exe"] = "Executables",
            [".msi"] = "Executables",
            [".dmg"] = "Executables",
            [".pkg"] = "Executables",
            [".deb"] = "Executables",
            [".rpm"] = "Executables",
            [".app"] = "Executables",

            // ── Fonts ─────────────────────────────────────────────────────────────
            [".ttf"]   = "Fonts",
            [".otf"]   = "Fonts",
            [".woff"]  = "Fonts",
            [".woff2"] = "Fonts",

            // ── Design / 3-D ──────────────────────────────────────────────────────
            [".psd"]    = "Design",
            [".ai"]     = "Design",
            [".xd"]     = "Design",
            [".sketch"] = "Design",
            [".fig"]    = "Design",
            [".blend"]  = "Design",
            [".obj"]    = "Design",
            [".fbx"]    = "Design",
        };

    /// <summary>Folder name used for extensions that have no explicit mapping.</summary>
    public const string OtherCategory = "Others";

    /// <summary>
    /// Returns the category folder name for the given file extension.
    /// Falls back to <see cref="OtherCategory"/> for unknown extensions.
    /// </summary>
    public static string GetCategory(string extension)
    {
        if (string.IsNullOrWhiteSpace(extension))
            return OtherCategory;

        return ExtensionToCategory.TryGetValue(extension, out var category)
            ? category
            : OtherCategory;
    }
}
