using System.Security.Cryptography;

namespace DirectoriesComparer;


public delegate void ProgressEventHandler(object? sender, CompareProgressEventArg arg);

public class DirComparer
{
    private DirInfo _primaryDir, _secondaryDir;

    public event ProgressEventHandler? OnProgressChanged;

    public DirComparer(DirInfo dir1, DirInfo dir2)
    {
        if (dir1.Files.Length >= dir2.Files.Length)
        {
            _primaryDir = dir1;
            _secondaryDir = dir2;
        }
        else
        {
            _primaryDir = dir2;
            _secondaryDir = dir1;
        }
    }

    public bool Compare()
    {
        var progress = 0f;
        var progressInc = 1f / _primaryDir.Files.Length;
        var isEqual = true;
        SetProgress(progress, "Start comparing...");
        var primaryList = _primaryDir.Files;

        foreach (var file in primaryList)
        {
            progress += progressInc;

            var primFile = Path.Combine(_primaryDir.BasePath, file);
            if (!File.Exists(primFile))
            {
                SetProgress(progress, $"File {primFile} not exist.");
                isEqual = false;
                continue;
            }

            var secFile = Path.Combine(_secondaryDir.BasePath, file);
            if (!File.Exists(secFile))
            {
                SetProgress(progress, $"File {secFile} not exist.");
                isEqual = false;
                continue;
            }

            var crc1 = GetFileCrc(primFile);
            var crc2 = GetFileCrc(secFile);
            if (!crc1.SequenceEqual(crc2))
            {
                SetProgress(progress, $"{primFile} is not equal to {secFile}");
                isEqual = false;
                continue;
            }

            SetProgress(progress);

        }

        return isEqual;
    }

    private void SetProgress(float progress, string message = "")
    {
        OnProgressChanged?.Invoke(this, new CompareProgressEventArg(progress, message));
    }

    private static byte[] GetFileCrc(string file)
    {
        using var stream = File.OpenRead(file);
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(stream);
    }
}