namespace futr.Models;

public partial class Civilization : BaseModel
{
    public string Faction { get; set; } = "";
    public string Date { get; set; } = "";

    public Dictionary<string, Datapoint> Datapoints = new();

    public Civilization(string id) : base(id)
    {
    }

    public new JsonPath.Node fromYaml(string yaml)
    {
        var node = base.fromYaml(yaml);

        Faction = node["faction"].AsString.Trim();
        Date = node["date"].AsString.Trim();

        var datapoints = node["metrics"].AsDictionary;
        foreach (var (metric, data) in datapoints) {
            var value = data["value"].AsString.Trim();
            var min = data["min"].AsString.Trim();
            var max = data["max"].AsString.Trim();
            var confidence = data["value"].AsString.Trim();

            var datapoint = new Datapoint(value, min, max, confidence);

            datapoint.Comment = data["comment"].AsString.Trim();
            datapoint.References = data["references"].AsList.Select(n => n.AsString).ToArray();

            Datapoints.Add(metric, datapoint);
        }

        return node;
    }
}
