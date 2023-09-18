namespace futr;

public class FutrApp
{
    public FutrConfig Config = new();
    public ICallbackLogger Log = new NullCallbackLogger();
    public FutrData Data = new FutrData();
}
