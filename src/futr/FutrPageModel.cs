using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace futr;

public class FutrPageModel : PageModel
{
    public FutrApp App;
    public ICallbackLogger Log;
    public FutrConfig Config;
    public string UiCultureName;
    public ITextProvider I18n;

    public FutrPageModel(FutrApp app, string textName)
    {
        App = app;
        Log = App.Log;
        Config = App.Config;
        UiCultureName = Thread.CurrentThread.CurrentUICulture.Name;
        I18n = new TextProvider(new ReadonlyFileDataProvider(), Config.AppName, UiCultureName, textName);
    }
}
