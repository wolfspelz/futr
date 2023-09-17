#nullable disable
using futr.GrainInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace futr.Pages;

public class CommandSymbols
{
    public string Delete { get; set; } = "&#10006;";
    public string Up { get; set; } = "&uArr;";
    public string Insert { get; set; } = "&#10095;"; // &#10095; // &#8690; // &#8628;
    public string Execute { get; set; } = "&#10097;&#10097;"; // &#10097;&#10097; // &larrhk;
    public string Save { get; set; } = "&#9733;"; // &#9733; // &#10029;
}

public class CommandDetail
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Template { get; set; }
    public string Arguments { get; set; }
    public bool ImmediateExecute { get; set; }
}

public class CommandResult
{
    public string Content { get; set; }
    public string ContentType { get; set; }

    public CommandResult(string content, string contentType)
    {
        Content = content;
        ContentType = contentType;
    }
}

public class CommandlineFavorites
{
    public List<KeyValuePair<string, string>> Favorites { get; set; }
    public CommandSymbols Symbols = new();

    public CommandlineFavorites(List<KeyValuePair<string, string>> favorites)
    {
        Favorites = favorites;
    }
}

public class CommandlineModel : FutrPageModel
{
    readonly ICommandline _commandline;
    private readonly IGrainFactory _grains;

    public readonly Dictionary<string, CommandDetail> Commands = new();
    public CommandSymbols Symbols = new();

    public CommandlineModel(ICommandline commandline, FutrApp app, IGrainFactory grains) : base(app, "Commandline")
    {
        _commandline = commandline;
        _grains = grains;

        if (_commandline is FutrCommandline futrCommandline) {
            futrCommandline.Grains = grains;
        }
    }

    public void InitCommandline()
    {
        _commandline.HttpContext = HttpContext;
    }

    public void OnGet()
    {
        InitCommandline();
        foreach (var pair in _commandline.GetHandlers()) {
            var handler = pair.Value;
            if (_commandline.IsAuthorizedForHandler(handler)) {
                Commands.Add(pair.Key, new CommandDetail {
                    Name = pair.Key,
                    Description = handler.Description,
                    Template = pair.Key + (pair.Value.Arguments == null ? "" :
                        pair.Value.ArgumentList == Commandline.ArgumentListType.KeyValue ?
                        pair.Value.Arguments.Aggregate(new StringBuilder(), (sb, x) => sb.Append(" " + x.Key + "="), sb => sb.ToString()) :
                        pair.Value.Arguments.Aggregate(new StringBuilder(), (sb, x) => sb.Append(" " + x.Key), sb => sb.ToString())
                        ),
                    Arguments = (pair.Value.Arguments == null ? "" : pair.Value.Arguments.Aggregate(new StringBuilder(), (sb, x) => sb.Append("[" + x.Key + ": " + x.Value + "] "), sb => sb.ToString())),
                    ImmediateExecute = pair.Value.ImmediateExecute,
                });
            }
        }
    }

    public PartialViewResult OnPostRun(string arg)
    {
        InitCommandline();
        var cmd = arg;
        try {
            var html = string.IsNullOrEmpty(cmd) ? "" : _commandline.Run(cmd);
            return Partial("_CommandlineResult", new CommandResult(html, "text/html"));
        } catch (Exception ex) {
            return Partial("_CommandlineResult", new CommandResult("<pre>" + string.Join(" | ", ex.GetMessages()) + "</pre>", "text/html"));
        }
    }

    public async Task<PartialViewResult> OnPostGetFavorites(string arg)
    {
        InitCommandline();
        try {
            var favoritesNode = await ReadFavorites();
            var model = await WriteFavorites(favoritesNode);
            var result = Partial("_CommandlineFavorites", model);
            return result;
        } catch (Exception ex) {
            return Partial("_CommandlineResult", new CommandResult("<pre>" + string.Join(" | ", ex.GetMessages()) + "</pre>", "text/html"));
        }
    }

    public async Task<PartialViewResult> OnPostSaveFavorite(string arg)
    {
        InitCommandline();
        var cmd = arg;
        try {
            var favoritesNode = await ReadFavorites();

            var key = RandomString.Alphanum(10);
            var newNode = new JsonPath.Node(JsonPath.Node.Type.Dictionary);
            newNode.AsDictionary.Add(key, cmd);
            favoritesNode.AsList.Add(newNode);

            var model = await WriteFavorites(favoritesNode);
            return Partial("_CommandlineFavorites", model);
        } catch (Exception ex) {
            return Partial("_CommandlineResult", new CommandResult("<pre>" + string.Join(" | ", ex.GetMessages()) + "</pre>", "text/html"));
        }
    }

    public async Task<PartialViewResult> OnPostDeleteFavorite(string arg)
    {
        InitCommandline();
        var key = arg;
        try {
            var favoritesNode = await ReadFavorites();

            var fav = favoritesNode.AsList.Where(node => node.AsDictionary.First().Key == key).FirstOrDefault();
            if (fav != null) {
                favoritesNode.AsList.Remove(fav);
            }

            var model = await WriteFavorites(favoritesNode);
            return Partial("_CommandlineFavorites", model);
        } catch (Exception ex) {
            return Partial("_CommandlineResult", new CommandResult("<pre>" + string.Join(" | ", ex.GetMessages()) + "</pre>", "text/html"));
        }
    }

    public async Task<PartialViewResult> OnPostUpFavorite(string arg)
    {
        InitCommandline();
        var key = arg;
        try {
            var favoritesNode = await ReadFavorites();

            var fav = favoritesNode.AsList.Where(node => node.AsDictionary.First().Key == key).FirstOrDefault();
            if (fav != null) {
                var idx = favoritesNode.AsList.FindIndex(node => node == fav);
                if (idx > 0) {
                    favoritesNode.AsList.Remove(fav);
                    favoritesNode.AsList.Insert(idx - 1, fav);
                }
            }

            var model = await WriteFavorites(favoritesNode);
            return Partial("_CommandlineFavorites", model);
        } catch (Exception ex) {
            return Partial("_CommandlineResult", new CommandResult("<pre>" + string.Join(" | ", ex.GetMessages()) + "</pre>", "text/html"));
        }
    }

    async Task<JsonPath.Node> ReadFavorites()
    {
        var favoritesJson = await _grains.GetGrain<ICachedString>("Web.Favorites." + GetUserName()).Get();
        if (string.IsNullOrEmpty(favoritesJson)) {
            favoritesJson = "[]";
        }
        var favoritesNode = JsonPath.Node.FromJson(favoritesJson);
        return favoritesNode;
    }

    async Task<CommandlineFavorites> WriteFavorites(JsonPath.Node favoritesNode)
    {
        var favoritesJson = favoritesNode.ToJson(spaced: true, indented: true);
        await _grains.GetGrain<ICachedString>("Web.Favorites." + GetUserName()).Set(favoritesJson, CachedStringOptions.Timeout.Infinite, CachedStringOptions.Persistence.Persistent);
        return new CommandlineFavorites(
            favoritesNode.AsList
            .Select(node => {
                var first = node.AsDictionary.First();
                return new KeyValuePair<string, string>(first.Key, first.Value.AsString);
            })
            .ToList()
        );
    }

    public string GetUserName()
    {
        var user = HttpContext.Session.GetString(FutrSession.Keys.User);
        return user ?? "";
    }
}
