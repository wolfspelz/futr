namespace futr;

public class FileStructureProvider : IDataStructureProvider
{
    public IEnumerable<string> EnumerateFiles(string root)
    {
        if (System.IO.Directory.Exists(root)) {
            return System.IO.Directory.EnumerateFiles(root);
        }
        return Array.Empty<string>();
    }

    public IEnumerable<string> EnumerateFolders(string root)
    {
        if (System.IO.Directory.Exists(root)) {
            return System.IO.Directory.EnumerateDirectories(root);
        }
        return Array.Empty<string>();
    }
}