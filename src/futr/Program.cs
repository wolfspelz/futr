using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using n3q.FrameworkTools;

namespace futr;

public class Program
{
    public static void Main(string[] args)
    {
        var myConfig = new FutrConfig();
        myConfig.Include(nameof(Config) + ".cs");

        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
        builder.Services.AddControllers();
        builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

        builder.Services.AddLocalization();
        builder.Services.AddMvc()
          .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
          .AddDataAnnotationsLocalization();

        builder.Services.Configure<RequestLocalizationOptions>(options => {
            var supportedCultures = new[] { "en-US", "de-DE" };
            options.SetDefaultCulture("de-DE")
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
        });

        var myApp = new FutrApp {
            Config = myConfig,
        };
        builder.Services.AddSingleton(myApp);

        var app = builder.Build();

        //var myLogger = new NullCallbackLogger();
        var myLogger = new MicrosoftLoggingCallbackLogger(app.Services.GetService<ILogger<Futr>>());
        app.Services.GetRequiredService<FutrApp>().Log = myLogger;

        myApp.Data.Log = myLogger;
        myApp.Data.Load(myConfig.DataFolder);

        // Configure the HTTP request pipeline.
        //if (!app.Environment.IsDevelopment()) {
        //    app.UseExceptionHandler("/Error");
        //}
        app.UseStaticFiles();

        var supportedCultures = new[]{
                new CultureInfo("en-US"),
                new CultureInfo("de-DE"),
            };
        var localizationOptions = new RequestLocalizationOptions {
            DefaultRequestCulture = new RequestCulture("en-US"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures,
        };
        localizationOptions.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider { QueryStringKey = "lang" });
        localizationOptions.RequestCultureProviders.Insert(1, new CookieRequestCultureProvider());
        localizationOptions.RequestCultureProviders.Insert(2, new AcceptLanguageHeaderRequestCultureProvider());
        app.UseRequestLocalization(localizationOptions); // Sets Thread.CurrentThread.CurrentUICulture

        app.UseRouteDebugger();

        app.UseCors();
        app.UseRouting();

        app.Use(next => context => {
            Console.WriteLine($"Middleware: Endpoint: {context.GetEndpoint()?.DisplayName}");
            return next(context);
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();
        app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

        app.UseForwardedHeaders(new ForwardedHeadersOptions {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.Run();
    }

    public class Futr { }
}
