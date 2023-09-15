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

    protected void AssertClaim(string role)
    {
        if (!HasRole(role)) {
            throw new UnauthorizedAccessException();
        }
    }

    protected bool HasRole(string role)
    {
        if (string.IsNullOrEmpty(role)) {
            throw new ArgumentNullException(nameof(role));
        }

        var roles = GetRoles();
        if (roles.Contains(role)) {
            return true;
        }

        return false;
    }

    protected IEnumerable<string> GetRoles()
    {
        var roles = HttpContext.Session.GetString(FutrSession.Keys.Roles);
        return FutrRoles.FromString(roles);
    }

    protected void SetRoles(IEnumerable<string>? roles = null)
    {
        if (roles == null) {
            roles = Array.Empty<string>();
        }

        if (roles.Count() == 0) {
            HttpContext.Session.Remove(FutrSession.Keys.Roles);
            return;
        }

        var rolesString = FutrRoles.AsString(roles);
        HttpContext.Session.SetString(FutrSession.Keys.Roles, rolesString);
    }

}
