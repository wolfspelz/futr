namespace futr.Models;

public class Metric
{
    public const string TagSeparator = ",";

    public string? Id { get; set; }
    public string? Title { get; set; }
    public string[] Tags = new string[0];
    public string Type { get; set; } = "";
    public string Unit { get; set; } = "";
    public string Range { get; set; } = "";
    public string Description { get; set; } = "";

    public Metric(string id)
    {
        Id = id;
        Title = id;
    }

    public Metric fromYaml(string yaml)
    {
        var root = JsonPath.Node.FromYaml(yaml, new YamlDeserializer.Options { LowerCaseDictKeys = true });

        Title = root["title"].AsString;
        Tags = root["tags"].AsList.Select(n => n.AsString).ToArray();
        Type = root["type"].AsString;
        Unit = root["unit"].AsString;
        Range = root["range"].AsString;
        Description = root["readme"].AsString;

        return this;
    }
}
