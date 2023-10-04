namespace futr.Models;

public class Universe : BaseModel
{
    public Dictionary<string, Civilization> Civilizations = new();
    public Dictionary<string, Faction> Factions = new();
    public string[] ShowcaseMetrics { get; private set; } = Array.Empty<string>();

    public Universe(string id) : base(id)
    {
    }

    public new JsonPath.Node fromYaml(string yaml)
    {
        var node = base.fromYaml(yaml);

        ShowcaseMetrics = node["showcasemetrics"].AsList.Select(n => n.AsString.Trim()).ToArray();

        return node;
    }
}
