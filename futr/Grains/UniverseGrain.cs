using Orleans.Runtime;
using futr.GrainInterfaces;

namespace futr.Grains;

public sealed class UniverseGrain : Grain, IUniverseGrain
{
    private readonly IPersistentState<UniverseState> _state;

    public UniverseGrain(
        [PersistentState(
            stateName: "Universe",
            storageName: MyGlobals.StorageName)]
            IPersistentState<UniverseState> state)
    {
        _state = state;
    }

    public Task<UniverseState> Get()
    {
        return Task.FromResult(_state.State);
    }

    public async Task Set(UniverseState universe)
    {
        universe.Id = this.GetPrimaryKeyString();
        _state.State = universe;
        await _state.WriteStateAsync();
    }

    public async Task Delete()
    {
        try {
            await _state.ClearStateAsync();
        } catch (Exception) {
            // Ignore non-existent state
        }

        this.DeactivateOnIdle();
    }
}
