using AutoMapper;

namespace futr;

public class FutrApp
{
    public FutrConfig Config = new();
    public ICallbackLogger Log = new NullCallbackLogger();
    public Mapper Mapper = new(new MapperConfiguration(config => { }));
}
