using AutoMapper;

namespace futr;

public class MyApp
{
    public MyConfig Config = new();
    public ICallbackLogger Log = new NullCallbackLogger();
    public Mapper Mapper = new(new MapperConfiguration(config => { }));
}
