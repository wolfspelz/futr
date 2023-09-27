namespace futr;

public interface IDataStructureProvider
{
    IEnumerable<string> EnumerateFolders(string root);
    IEnumerable<string> EnumerateFiles(string root);
}