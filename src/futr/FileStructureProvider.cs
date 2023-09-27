namespace futr;

public class FileStructureProvider : IDataStructureProvider
{
    public IEnumerable<string> EnumerateFiles(string root)
    {
        return System.IO.Directory.EnumerateFiles(root);
    }

    public IEnumerable<string> EnumerateFolders(string root)
    {
        return System.IO.Directory.EnumerateDirectories(root);
    }
}