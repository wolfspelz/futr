using System.Security.Cryptography.Xml;

namespace futr.Models;

public class BaseModel
{
    public const string TagSeparator = ",";

    public string? Id { get; set; }
    public string? Title { get; set; }
    public List<string> Tags = new();
    public double Order { get; set; } = 0.0;
    public string Tile { get; set; } = "";
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

        var order = node["order"].AsString.Trim();
        if (order != "") {
            Order = node["order"].AsFloat;
        }

        Tile = node["tile"].AsString;
        Readme = node["readme"].AsString;
        Icons = node["icons"].AsList.Select(n => n.AsString).ToList();
        Editors = node["editors"].AsList.Select(n => n.AsString).ToList();
        Approvers = node["approvers"].AsList.Select(n => n.AsString).ToList();

        {
            var imageList = node["images"].AsList;
            foreach (var imageNode in imageList) {
                var image = new ImageModel();
                if (imageNode.IsDictionary) {
                    image.Link = imageNode["link"].AsString;
                    image.Text = imageNode["text"].AsString;
                    image.Page = imageNode["page"].AsString;
                } else {
                    image.Link = imageNode.AsString;
                }
                if (image.Page == "") {
                    image.Page = image.Link;
                }
                Images.Add(image);
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
