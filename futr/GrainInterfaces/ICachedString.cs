namespace futr.GrainInterfaces;

[GenerateSerializer]
public class CachedStringState
{
    [Id(0)]
    public string? Data;

    [Id(1)]
    public long Expires;
}

public static class CachedStringOptions
{
    public static class Timeout
    {
        public const long Infinite = -1;
    }

    public enum Persistence
    {
        Transient,
        Persistent,
    }
}

public interface ICachedString : IGrainWithStringKey
{
    Task Set(string s, long timeout = CachedStringOptions.Timeout.Infinite, CachedStringOptions.Persistence persistence = CachedStringOptions.Persistence.Transient);
    Task<string?> Get();
    Task Delete();

    Task SetTime(DateTime time);
    Task Deactivate();
    Task DeletePersistentStorage();
    Task ReloadPersistentStorage();
}
