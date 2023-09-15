using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace futr.Pages
{
    public class AuthModel : FutrPageModel
    {
        private readonly IGrainFactory _grains;

        public bool IsLoggedIn { get; set; } = false;

        public AuthModel(FutrApp app, IGrainFactory grains) : base(app, "Auth")
        {
            _grains = grains;
        }

        public void OnGet()
        {
            var user = HttpContext.Session.GetString(FutrSession.Keys.User);
            if (Is.Value(user)) {
                IsLoggedIn = true;
            }
        }

        public IActionResult OnPostLogin(string token)
        {
            if (Is.Value(token)) {
                if (Config.AccessTokens.ContainsKey(token)) {
                    var accessToken = Config.AccessTokens[token];
                    SetRoles(accessToken.Roles);
                    HttpContext.Session.SetString(FutrSession.Keys.User, accessToken.User);
                    IsLoggedIn = true;
                }
            }

            //if (IsLoggedIn) {
            //    Response.Cookies.Append(AccessTokenCookieName, token, new Microsoft.AspNetCore.Http.CookieOptions { Secure = true });
            //    return RedirectToPage("/Blog/Index");
            //}

            return RedirectToPage();
        }

        public IActionResult OnPostLogout()
        {
            SetRoles();
            HttpContext.Session.Remove(FutrSession.Keys.User);
            IsLoggedIn = true;
            return RedirectToPage();
        }

    }
}
