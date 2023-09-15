using Orleans.Runtime;
using futr.GrainInterfaces;

namespace futr.Grains;

public sealed class CachedStringGrain : Grain, ICachedString
{
    private readonly IPersistentState<GrainInterfaces.CachedStringState> _state;

    public string? Data
    {
        get { return _state.State.Data; }
        set { _state.State.Data = value; }
    }

    public long Expires
    {
        get { return _state.State.Expires; }
        set { _state.State.Expires = value; }
    }

    public DateTime Time { get; set; } = DateTime.MinValue;
    public CachedStringOptions.Persistence Persistence { get; set; } = CachedStringOptions.Persistence.Transient;

    public CachedStringGrain(
        [PersistentState(
            stateName: "CachedString",
            storageName: FutrGlobals.StorageName)]
            IPersistentState<GrainInterfaces.CachedStringState> state)
    {
        _state = state;
    }

    #region Interface

    public async Task Set(string data, long timeout = CachedStringOptions.Timeout.Infinite, CachedStringOptions.Persistence persistence = CachedStringOptions.Persistence.Transient)
    {
        Data = data;
        Persistence = persistence;

        if (timeout == CachedStringOptions.Timeout.Infinite) {
            Expires = 0;
        } else {
            Expires = GetCurrentTime().AddSeconds(timeout).ToLong();
        }

        if (Persistence == CachedStringOptions.Persistence.Persistent) {
            await _state.WriteStateAsync();
        }
    }

    public async Task<string?> Get()
    {
        string result = "";

        if (Data != null) {
            if (Expires == 0) {
                result = Data;
            } else {
                if (GetCurrentTime() < Expires.ToDateTime()) {
                    result = Data;
                } else {
                    if (Persistence == CachedStringOptions.Persistence.Persistent) {
                        await _state.ClearStateAsync();
                    }
                }
            }
        }

        return result;
    }

    public async Task Delete()
    {
        bool wasSet = (Data != null);

        Data = null;
        Expires = 0;

        await _state.ClearStateAsync();
    }

    #endregion

    #region Internal

    DateTime GetCurrentTime()
    {
        return (Time == DateTime.MinValue) ? DateTime.UtcNow : Time;
    }

    #endregion

    #region For tests

    public Task SetTime(DateTime time)
    {
        Time = time;
        return Task.CompletedTask;
    }

    public async Task Deactivate()
    {
        this.DeactivateOnIdle();
        await Task.CompletedTask;
    }

    public async Task DeletePersistentStorage()
    {
        try {
            await _state.ClearStateAsync();
        } catch (Exception) {
            // Ignore non-existent state
        }
    }

    public async Task ReloadPersistentStorage()
    {
        await _state.ReadStateAsync();
    }

    #endregion
}
