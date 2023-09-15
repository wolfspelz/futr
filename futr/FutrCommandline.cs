#nullable disable
namespace futr;

public class FutrCommandline: Commandline, ICommandline
{
    public FutrConfig Config { get; }
    public ICallbackLogger Log { get; }
    public HttpContext HttpContext { get; set; }

    public FutrCommandline(FutrApp app)
    {
        Config = app.Config;
        Log = app.Log;
    }
}
