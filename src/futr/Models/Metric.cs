namespace futr.Models;

public class Metric : BaseModel
{
    public string Type { get; set; } = "";
    public string Unit { get; set; } = "";
    public string Range { get; set; } = "";

    public Metric(string id) : base(id)
    {
    }

    public new JsonPath.Node fromYaml(string yaml)
    {
        var node = base.fromYaml(yaml);

        Type = node["type"].AsString.Trim();
        Unit = node["unit"].AsString.Trim();
        Range = node["range"].AsString.Trim();

        return node;
    }
}
