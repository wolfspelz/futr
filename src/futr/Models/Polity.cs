namespace futr.Models;

public class Polity : BaseModel
{
    public Universe Universe { get; set; }

    //public string Type { get; set; } = "";

    public Polity(string id, Universe universe) : base(id)
    {
        Universe = universe;
    }

    public new JsonPath.Node fromYaml(string yaml)
    {
        var node = base.fromYaml(yaml);

        if (Editors.Count == 0) {
            Editors = Universe.Editors;
        }
        if (Approvers.Count == 0) {
            Approvers = Universe.Approvers;
        }

        //Type = node["type"].AsString.Trim();

        return node;
    }
}
