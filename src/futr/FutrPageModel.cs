using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using n3q.FrameworkTools;

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
        I18n = new TextProvider(new ReadonlyFileDataProvider(), Config.AppAcronym, UiCultureName, textName);
    }

    public override async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
    {
        try {
            var data = new LogData();

            if (context.HandlerArguments.Count > 0) {
                data[LogData.Key.Arguments] = new LogData(context.HandlerArguments);
            }

            var request = context.HttpContext.Request;
            data[LogData.Key.Uri] = request.Method
                + " " + request.Path + request.QueryString
                + " " + request.Protocol
                ;
            Log.SetTraceIdentifier(context.HttpContext);
            Log.Info($"{nameof(FutrPageModel)}", data, context.ActionDescriptor.DisplayName, "");
        } catch (Exception ex) {
            Log.Error(ex);
        }

        await next.Invoke();
    }

}
