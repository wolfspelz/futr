namespace futr.GrainInterfaces;

public interface IUniverseGrain : IGrainWithStringKey
{
    Task<UniverseState> Get();
    Task Set(UniverseState universe);
    Task Delete();
}