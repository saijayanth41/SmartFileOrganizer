# 🗂️ Smart File Organizer

A **C# .NET 10** console application that automatically categorizes and organizes files from a given folder into clean subfolders based on file extension.

---

## 📌 Problem Statement

Users often accumulate hundreds of mixed files in a single folder — Downloads, Desktop, shared drives. Manually sorting them is repetitive, time-consuming, and error-prone. This tool automates the entire process.

---

## 🎯 Objective

Scan a directory, identify file extensions, and move files into clearly labeled category subfolders — safely, efficiently, and with full control.

---

## 🚀 Features

- ✅ **Dry Run** — preview every planned move before touching any file
- ✅ **13 categories** — Images, Documents, Videos, Audio, Code, Archives, and more
- ✅ **100+ extensions** mapped out of the box
- ✅ **Conflict resolution** — Rename / Skip / Overwrite strategies
- ✅ **Re-run safe** — skips files already inside a category subfolder
- ✅ **Recursive mode** — optionally include subdirectories
- ✅ **Memory efficient** — streams files with `yield return`, never loads all paths at once
- ✅ **Clean architecture** — interface-driven, fully extensible

---

## 🗃️ Supported Categories

| Category      | Extensions |
|---------------|------------|
| Images        | `.jpg` `.jpeg` `.png` `.gif` `.bmp` `.svg` `.webp` `.ico` `.tiff` `.heic` `.raw` |
| Documents     | `.pdf` `.doc` `.docx` `.txt` `.rtf` `.odt` `.pages` `.md` `.epub` |
| Spreadsheets  | `.xls` `.xlsx` `.csv` `.ods` `.numbers` |
| Presentations | `.ppt` `.pptx` `.odp` `.key` |
| Videos        | `.mp4` `.mkv` `.avi` `.mov` `.wmv` `.flv` `.webm` `.m4v` `.mpg` |
| Audio         | `.mp3` `.wav` `.flac` `.aac` `.ogg` `.wma` `.m4a` `.aiff` |
| Archives      | `.zip` `.rar` `.7z` `.tar` `.gz` `.bz2` `.xz` |
| Code          | `.cs` `.py` `.js` `.ts` `.html` `.css` `.java` `.cpp` `.go` `.rs` `.swift` `.kt` `.sh` `.sql` `.json` `.yaml` |
| Executables   | `.exe` `.msi` `.dmg` `.pkg` `.deb` `.rpm` `.app` |
| Fonts         | `.ttf` `.otf` `.woff` `.woff2` |
| Design        | `.psd` `.ai` `.xd` `.sketch` `.fig` `.blend` `.obj` `.fbx` |
| Others        | Anything not listed above |

---

## 🏗️ Project Structure

```
SmartFileOrganizer/
├── SmartFileOrganizer.csproj
├── Program.cs                      ← CLI entry point & output rendering
├── Models/
│   ├── FileEntry.cs                ← Represents one scanned file
│   ├── OrganizationConfig.cs       ← Run-time settings + ConflictStrategy enum
│   └── OrganizationResult.cs       ← Moved / Skipped / Failed records
├── Configuration/
│   └── CategoryMapper.cs           ← Extension → folder name mapping
└── Services/
    ├── Interfaces/
    │   ├── IFileScanner.cs
    │   ├── IFileOrganizer.cs
    │   └── IConflictResolver.cs
    ├── FileScanner.cs              ← Streams files using deferred yield
    ├── ConflictResolver.cs         ← Appends _1, _2 … to avoid collisions
    └── FileOrganizer.cs            ← Core move logic + conflict dispatch
```

---

## ⚙️ Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- macOS (Apple Silicon or Intel)
- VS Code + [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) *(recommended)*

Install .NET via Homebrew:
```bash
brew install --cask dotnet-sdk
```

---

## 🔧 Build & Run

```bash
# Clone / open the project
cd /Users/saijayanth/Desktop/Splunk

# Build
dotnet build

# Preview (no files moved)
dotnet run -- ~/Downloads --dry-run

# Organize
dotnet run -- ~/Downloads

# Path with spaces — wrap in quotes
dotnet run -- "/Users/saijayanth/Desktop/Screen shots"
```

---

## 📋 CLI Options

| Option | Default | Description |
|--------|---------|-------------|
| `--dry-run` | off | Preview all moves without touching the filesystem |
| `--recursive` | off | Also organize files inside subdirectories |
| `--include-hidden` | off | Include hidden files |
| `--conflict Rename` | ✅ on | Append `_1`, `_2` … to avoid name collisions |
| `--conflict Skip` | off | Leave source file untouched if a collision occurs |
| `--conflict Overwrite` | off | Replace the existing file at the destination |

---

## 💡 Usage Examples

```bash
# 1. Preview Downloads
dotnet run -- ~/Downloads --dry-run

# 2. Organize Downloads
dotnet run -- ~/Downloads

# 3. Recursive + skip conflicts
dotnet run -- ~/Downloads --recursive --conflict Skip

# 4. Run from a different directory
dotnet run --project /Users/saijayanth/Desktop/Splunk -- ~/Downloads --dry-run
```

---

## 📦 Publish as a Standalone Executable

```bash
# Apple Silicon (M1/M2/M3)
dotnet publish -c Release -r osx-arm64 --self-contained -o ./publish

# Intel Mac
dotnet publish -c Release -r osx-x64 --self-contained -o ./publish

# Run directly (no dotnet runtime needed)
./publish/SmartFileOrganizer ~/Downloads --dry-run
```

---

## 🔁 Exit Codes

| Code | Meaning |
|------|---------|
| `0` | Success — all files processed |
| `1` | Invalid arguments or directory not found |
| `2` | One or more files failed to move |
