using Microsoft.AspNetCore.Mvc;

namespace futr;

public class FutrControllerBase : Controller
{
    public FutrApp App;
    public ICallbackLogger Log;
    public FutrConfig Config;
    public string UiCultureName;

    public FutrControllerBase(FutrApp app)
    {
        App = app;
        Log = App.Log;
        Config = App.Config;
        UiCultureName = Thread.CurrentThread.CurrentUICulture.Name;
    }
}
