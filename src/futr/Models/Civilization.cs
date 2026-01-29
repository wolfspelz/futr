namespace futr.Models;

public class Civilization : BaseModel
{
    public Universe Universe { get; set; }

    public string Polity { get; set; } = "";

    private string _date = "";
    public long Year { get; set; } = long.MinValue;
    public string Date
    {
        get {
            return _date;
        }
        set {
            if (long.TryParse(value, out var year)) {
                Year = year;
            } else if (double.TryParse(value, out var yearDouble)) {
                Year = (long)yearDouble;
            }
            _date = value;
        }
    }

    public Dictionary<string, Datapoint> Datapoints = new();

    public Civilization(string id, Universe universe) : base(id)
    {
        Universe = universe;
    }

    public new JsonPath.Node fromYaml(string yaml)
    {
        Order = Universe.Order;

        var node = base.fromYaml(yaml);

        if (Editors.Count == 0) {
            Editors = Universe.Editors;
        }

        Polity = node["polity"].AsString.Trim();
        Date = node["date"].AsString.Trim();

        return node;
    }
}
