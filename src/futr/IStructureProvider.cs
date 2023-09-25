namespace futr;

public interface IStructureProvider
{
    IEnumerable<string> EnumerateFolders(string root);
    IEnumerable<string> EnumerateFiles(string root);
}