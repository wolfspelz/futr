namespace futr.Models;

public partial class Civilization : BaseModel
{
    public Universe Universe { get; set; }

    public string Faction { get; set; } = "";
    public string Date { get; set; } = "";

    public List<Datapoint> Datapoints = new();

    public Civilization(Universe universe, string id) : base(id)
    {
        Universe = universe;
    }

    public new JsonPath.Node fromYaml(string yaml)
    {
        var node = base.fromYaml(yaml);

        Faction = node["faction"].AsString.Trim();
        Date = node["date"].AsString.Trim();

        var datapoints = node["datapoints"].AsList;
        foreach (var data in datapoints) {
            var metricId = data["metric"].AsString.Trim();
            var value = data["value"].AsString.Trim();
            var min = data["min"].AsString.Trim();
            var max = data["max"].AsString.Trim();
            var confidence = data["confidence"].AsString.Trim();

            var datapoint = new Datapoint(metricId, value, min, max, confidence);

            datapoint.Comment = data["comment"].AsString.Trim();
            datapoint.References = data["references"].AsList.Select(n => n.AsString).ToArray();

            Datapoints.Add(datapoint);
        }

        return node;
    }
}
