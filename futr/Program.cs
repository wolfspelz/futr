using AutoMapper;
using futr.GrainInterfaces;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using Orleans.Configuration;
using Orleans.Storage;
using System.Text.Json;

namespace futr;

public class Program
{
    public class SystemTextJsonSerializer : IGrainStorageSerializer
    {
        public BinaryData Serialize<T>(T input)
        {
            return new BinaryData(JsonSerializer.SerializeToUtf8Bytes(input));
        }

        public T Deserialize<T>(BinaryData input)
        {
            return input.ToObjectFromJson<T>()!;
        }
    }

    public static void Main(string[] args)
    {
        var myConfig = new FutrConfig();
        myConfig.Include(nameof(BaseConfig) + ".cs");

        var myLogger = new NullCallbackLogger();

        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseOrleans(siloBuilder => {
            siloBuilder.UseLocalhostClustering(
                myConfig.SiloPort,
                myConfig.GatewayPort,
                null,
                myConfig.ServiceId,
                myConfig.ClusterId
            );

            //siloBuilder.AddAzureTableGrainStorage(
            //    name: MyGlobals.StorageName,
            //    configureOptions: options => {
            //        options.ConfigureTableServiceClient(myConfig.AzureTableConnectionString);
            //        options.GrainStorageSerializer = new SystemTextJsonSerializer();
            //    }
            //);

            siloBuilder.AddCosmosGrainStorage(
                FutrGlobals.StorageName,
                builder => builder.Configure<IOptions<ClusterOptions>>((options, silo) => {
                    options.ConfigureCosmosClient(myConfig.CosmosDbConnectionString);
                    options.IsResourceCreationEnabled = true;
                })
            );
        });

        builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
        builder.Services.AddControllers();
        builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

        builder.Services.AddLocalization();
        builder.Services.AddMvc()
          .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
          .AddDataAnnotationsLocalization();

        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromSeconds(10);
            options.Cookie.HttpOnly = true;
            //options.Cookie.IsEssential = true;
        });

        builder.Services.Configure<RequestLocalizationOptions>(options => {
            var supportedCultures = new[] { "en-US", "de-DE" };
            options.SetDefaultCulture("de-DE")
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
        });

        var mapper = new Mapper(
            new MapperConfiguration(cfg => {
                cfg.CreateMap<UniverseState, Universe>()
                    .ForMember(dest => dest.Tags, act => act.MapFrom(src => String.Join(Universe.TagSeparator + " ", src.Tags)));
                cfg.CreateMap<Universe, UniverseState>()
                    .ForMember(dest => dest.Tags, act => act.MapFrom(src => src.Tags.Split(Universe.TagSeparator, StringSplitOptions.None).Select(x => x.Trim()).ToArray()));
            })
        );

        var myApp = new FutrApp {
            Config = myConfig,
            Log = myLogger,
            Mapper = mapper,
        };
        builder.Services.AddSingleton(myApp);

        var app = builder.Build();

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

        app.UseCors();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSession();

        app.MapRazorPages();
        app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
        
        app.UseForwardedHeaders(new ForwardedHeadersOptions {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.Run();
    }

}
