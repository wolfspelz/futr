namespace futr.Models;

public class ImageModel
{
    public string Link { get; internal set; } = "";
    public string Caption { get; internal set; } = "";
}

public class BaseModel
{
    public const string TagSeparator = ",";

    public string? Id { get; set; }
    public string? Title { get; set; }
    public List<string> Tags = new();
    public string Order { get; set; } = "m";
    public string Tile { get; set; } = "";
    public string Description { get; set; } = "";
    public List<string> Icons = new();
    public List<ImageModel> Images = new();
    public List<string> Editors = new();
    public List<string> Approvers = new();
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

        Tags = node["tags"].AsList.Select(n => n.AsString.Trim()).ToList();
        Description = node["readme"].AsString;
        Tile = node["tile"].AsString;
        Icons = node["icons"].AsList.Select(n => n.AsString).ToList();
        Editors = node["editors"].AsList.Select(n => n.AsString).ToList();
        Approvers = node["approvers"].AsList.Select(n => n.AsString).ToList();

        //Images = node["images"].AsList.Select(n => n.AsString).ToList();
        var imageList = node["images"].AsList;
        foreach (var imageNode in imageList)
        {
            var image = new ImageModel();
            if (imageNode.IsDictionary) {
                image.Link = imageNode["link"].AsString;
                image.Caption = imageNode["caption"].AsString;
            } else {
                image.Link = imageNode.AsString;
            }
            Images.Add(image);
        }

        return node;
    }
}
