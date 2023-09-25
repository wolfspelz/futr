namespace futr;

public class FileStructureProvider : IStructureProvider
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