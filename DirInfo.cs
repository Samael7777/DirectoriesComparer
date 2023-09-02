namespace DirectoriesComparer;

public class DirInfo
{
    public string BasePath { get; }
    public string[] Files => GetFiles();

    public DirInfo(string basePath)
    {
        BasePath = basePath;
    }

    private string[] GetFiles()
    {
        var list = Directory.GetFiles(BasePath, "*.*", SearchOption.AllDirectories);
        var files = new string[list.Length];
        for (var i = 0; i < list.Length; i++)
        {
            var fullPath = list[i];
            var relativePath = Path.GetRelativePath(BasePath, fullPath);
            files[i] = relativePath;
        }

        return files;
    }
}