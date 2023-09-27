namespace futr.Models;

public class Faction : BaseModel
{
    //public string Type { get; set; } = "";

    public Faction(string id) : base(id)
    {
    }

    public new JsonPath.Node fromYaml(string yaml)
    {
        var node = base.fromYaml(yaml);

        //Type = node["type"].AsString.Trim();

        return node;
    }
}
