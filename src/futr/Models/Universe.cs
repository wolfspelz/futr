namespace futr.Models;

public class Universe : BaseModel
{
    public Dictionary<string, Civilization> Civilizations = new();

    public Universe(string id) : base(id)
    {
    }

    public new JsonPath.Node fromYaml(string yaml)
    {
        var node = base.fromYaml(yaml);

        //Type = node["type"].AsString.Trim();

        return node;
    }
}
