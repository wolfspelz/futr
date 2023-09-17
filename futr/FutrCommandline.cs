#nullable disable
using AutoMapper;
using Azure.Core;
using futr.GrainInterfaces;

namespace futr;

public class FutrCommandline: Commandline, ICommandline
{
    public FutrConfig Config { get; }
    public ICallbackLogger Log { get; }
    public Mapper Mapper { get; }
    public HttpContext HttpContext { get; set; }
    public IGrainFactory Grains { get; set; }

    public FutrCommandline(FutrApp app)
    {
        Config = app.Config;
        Log = app.Log;
        Mapper = app.Mapper;

        Handlers.Add(nameof(Universe_Set), new Handler { Name = nameof(Universe_Set), Function = Universe_Set, Role = nameof(Role.CommandlineAdmin), Description = "Set/create Universe", ArgumentList = ArgumentListType.Tokens, Arguments = new ArgumentDescriptionList { ["Id"] = "Id", ["Name"] = "Name", ["Description"] = "Markdown description", ["Tags"] = "Comma separated list of tags" } });
        Handlers.Add(nameof(Universe_Get), new Handler { Name = nameof(Universe_Get), Function = Universe_Get, Role = nameof(Role.CommandlineAdmin), Description = "Show Universe", ArgumentList = ArgumentListType.Tokens, Arguments = new ArgumentDescriptionList { ["Id"] = "Id" } });
        Handlers.Add(nameof(Universe_Deactivate), new Handler { Name = nameof(Universe_Deactivate), Function = Universe_Deactivate, Role = nameof(Role.CommandlineAdmin), Description = "Remove Universe grain from memory", ArgumentList = ArgumentListType.Tokens, Arguments = new ArgumentDescriptionList { ["Id"] = "Id" } });
        Handlers.Add(nameof(Universe_Delete), new Handler { Name = nameof(Universe_Delete), Function = Universe_Delete, Role = nameof(Role.CommandlineAdmin), Description = "Delete Universe", ArgumentList = ArgumentListType.Tokens, Arguments = new ArgumentDescriptionList { ["Id"] = "Id" } });
    }

    public override bool IsAuthorizedForHandler(Handler handler)
    {
        var rolesString = HttpContext.Session.GetString(FutrSession.Keys.Roles);
        var roles = FutrRoles.FromString(rolesString);
        if (roles.Contains(handler.Role)) {
            return true;
        }
        return handler.Role == nameof(Role.CommandlinePublic);
    }

    #region Universe

    object Universe_Set(Arglist args)
    {
        args.Next("cmd");
        var id = args.Next("Id");
        var name = args.Next("Name");
        var description = args.Next("Description", "");
        var tags = args.Next("Tags", "");
        var universe = new Universe {
            Id =id,
            Name = name,
            Description = description, 
            Tags = tags,
        };
        var universeState = Mapper.Map<UniverseState>(universe);
        Grains.GetGrain<IUniverseGrain>(id).Set(universeState).Wait();
        return $"Domain={universe.Id}";
    }

    object Universe_Get(Arglist args)
    {
        args.Next("cmd");
        var id = args.Next("Id");
        var universe = Mapper.Map<Universe>(Grains.GetGrain<IUniverseGrain>(id).Get().Result);
        return $"id={universe.Id} Name={universe.Name} Description={universe.Description} Tags={universe.Tags}";
    }

    object Universe_Deactivate(Arglist args)
    {
        args.Next("cmd");
        var id = args.Next("Id");
        Grains.GetGrain<IUniverseGrain>(id).Deactivate().Wait();
        return $"Deactivated";
    }

    object Universe_Delete(Arglist args)
    {
        args.Next("cmd");
        var id = args.Next("Id");
        Grains.GetGrain<IUniverseGrain>(id).Delete().Wait();
        return $"Deleted";
    }

    #endregion

}
