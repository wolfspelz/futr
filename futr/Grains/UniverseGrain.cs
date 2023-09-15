using Orleans.Runtime;
using futr.GrainInterfaces;

namespace futr.Grains;

public sealed class UniverseGrain : Grain, IUniverseGrain
{
    private readonly IPersistentState<GrainInterfaces.UniverseState> _state;

    public UniverseGrain(
        [PersistentState(
            stateName: "Universe",
            storageName: FutrGlobals.StorageName)]
            IPersistentState<GrainInterfaces.UniverseState> state)
    {
        _state = state;
    }

    public Task<GrainInterfaces.UniverseState> Get()
    {
        return Task.FromResult(_state.State);
    }

    public async Task Set(GrainInterfaces.UniverseState universe)
    {
        universe.Id = this.GetPrimaryKeyString();
        _state.State = universe;
        await _state.WriteStateAsync();
    }

    public async Task Delete()
    {
        await DeleteStorage();
        await InvalidateMemory();
    }

    public async Task DeleteStorage()
    {
        try {
            await _state.ClearStateAsync();
        } catch (Exception) {
            // Ignore non-existent state
        }
    }

    public async Task InvalidateMemory()
    {
        this.DeactivateOnIdle();
        await Task.CompletedTask;
    }
}
