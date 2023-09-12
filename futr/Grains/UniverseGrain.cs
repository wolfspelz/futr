using Orleans.Runtime;
using futr.GrainInterfaces;

namespace futr.Grains;

public sealed class UniverseGrain : Grain, IUniverseGrain
{
    private readonly IPersistentState<Universe> _state;

    public UniverseGrain(
        [PersistentState(
            stateName: "Universe",
            storageName: MyGlobals.StorageName)]
            IPersistentState<Universe> state)
    {
        _state = state;
    }

    public async Task Set(string id, Universe universe)
    {
        _state.State = universe;
        await _state.WriteStateAsync();
    }

    public Task<Universe> Get()
    {
        return Task.FromResult(_state.State);
    }
}
