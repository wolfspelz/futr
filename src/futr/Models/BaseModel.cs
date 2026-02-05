using System.Security.Cryptography.Xml;

namespace futr.Models;

public class BaseModel
{
    public const string TagSeparator = ",";

    public string? Id { get; set; }
    public string? Title { get; set; }
    public DateTime Created { get; set; } = DateTime.MinValue;
    public DateTime Changed { get; set; } = DateTime.MinValue;
    public List<string> Tags = new();
    public double Order { get; set; } = 0.0;
    public ImageModel? Tile { get; set; }
    public string Readme { get; set; } = "";
    public List<string> Icons = new();
    public List<ImageModel> Images = new();
    public List<LinkModel> Links = new();
    public List<ReferenceModel> References = new();
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

        Tags = node["tags"].AsList.Select(n => n.AsString.Trim()).ToList();

        var created = node["created"].AsString.Trim();
        if (created != "" && DateTime.TryParse(created, out var createdDate)) {
            Created = createdDate;
        }

        var changed = node["changed"].AsString.Trim();
        if (changed != "" && DateTime.TryParse(changed, out var changedDate)) {
            Changed = changedDate;
        }

        var order = node["order"].AsString.Trim();
        if (order != "") {
            Order = node["order"].AsFloat;
        }

        Readme = node["readme"].AsString;
        Icons = node["icons"].AsList.Select(n => n.AsString).ToList();
        Editors = node["editors"].AsList.Select(n => n.AsString).ToList();
        Approvers = node["approvers"].AsList.Select(n => n.AsString).ToList();

        {
            var imageList = node["images"].AsList;
            foreach (var imageNode in imageList) {
                var image = new ImageModel();
                if (imageNode.IsDictionary) {
                    image.Src = imageNode["src"].AsString;
                    image.Text = imageNode["text"].AsString;
                    image.Link = imageNode["link"].AsString;
                    image.Author = imageNode["author"].AsString;
                    image.License = imageNode["license"].AsString;
                    image.Legal = imageNode["legal"].AsString;
                    image.Permission = imageNode["permission"].AsString;
                    image.Tags = imageNode["tags"].AsList.Select(n => n.AsString).ToList();
                } else {
                    image.Src = imageNode.AsString;
                }
                if (image.Link == "") {
                    image.Link = image.Src;
                }
                Images.Add(image);
                if (image.Tags.Contains("main")) {
                    Tile = image;
                }
            }
            if (Tile == null && Images.Count > 0) {
                Tile = Images[0];
            }
        }

        {
            var linkList = node["links"].AsList;
            foreach (var imageNode in linkList) {
                var link = new LinkModel();
                if (imageNode.IsDictionary) {
                    link.Link = imageNode["link"].AsString;
                    link.Text = imageNode["text"].AsString;
                } else {
                    link.Link = imageNode.AsString;
                    link.Text = link.Link;
                }
                Links.Add(link);
            }
        }

        {
            var referenceList = node["references"].AsList;
            foreach (var referenceNode in referenceList) {
                var reference = new ReferenceModel();
                if (referenceNode.IsDictionary) {
                    reference.Link = referenceNode["link"].AsString;
                    reference.Text = referenceNode["text"].AsString;
                } else {
                    reference.Link = referenceNode.AsString;
                    reference.Text = reference.Link;
                }
                References.Add(reference);
            }
        }

        return node;
    }
}
