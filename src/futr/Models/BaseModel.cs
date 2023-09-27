namespace futr.Models;

public class BaseModel
{
    public const string TagSeparator = ",";

    public string? Id { get; set; }
    public string? Title { get; set; }
    public string[] Tags = new string[0];
    public string Description { get; set; } = "";
    public string[] Icons = new string[0];
    public string[] Images = new string[0];
    public string Error { get; set; } = "";

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

        Tags = node["tags"].AsList.Select(n => n.AsString.Trim()).ToArray();
        Description = node["readme"].AsString;
        Icons = node["icons"].AsList.Select(n => n.AsString).ToArray();
        Images = node["images"].AsList.Select(n => n.AsString).ToArray();

        return node;
    }
}
