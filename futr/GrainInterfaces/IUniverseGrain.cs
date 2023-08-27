namespace futr.GrainInterfaces;

public interface IUniverseGrain : IGrainWithStringKey
{
    Task Set(string id, Universe universe);
    Task<Universe> Get();
}