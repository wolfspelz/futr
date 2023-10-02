namespace futr.Models;

public class BaseModel
{
    public const string TagSeparator = ",";

    public string? Id { get; set; }
    public string? Title { get; set; }
    public string[] Tags = Array.Empty<string>();
    public string Order { get; set; } = "m";
    public string Description { get; set; } = "";
    public string[] Icons = Array.Empty<string>();
    public string[] Images = Array.Empty<string>();
    public string[] Editors = Array.Empty<string>();
    public string[] Approvers = Array.Empty<string>();
    public string Error { get; set; } = "";
    public string SeoName { get; set; } = "";

    protected JsonPath.Node? node { get; set; }

    public BaseModel(string id)
    {
        Id = id;
        Title = id;
    }

    public JsonPath.Node fromYaml(string yaml)
    {
        var node = Node.FromYaml(yaml, new YamlDeserializer.Options { LowerCaseDictKeys = true });

        var title = node["title"].AsString.Trim();
        if (title != "") {
            Title = title;
        }

        var order = node["order"].AsString.Trim();
        if (order == "") {
            order = "m";
        }
        Order = order + "-" + Title;

        Tags = node["tags"].AsList.Select(n => n.AsString.Trim()).ToArray();
        Description = node["readme"].AsString;
        Icons = node["icons"].AsList.Select(n => n.AsString).ToArray();
        Images = node["images"].AsList.Select(n => n.AsString).ToArray();
        Editors = node["editors"].AsList.Select(n => n.AsString).ToArray();
        Approvers = node["approvers"].AsList.Select(n => n.AsString).ToArray();

        return node;
    }
}
