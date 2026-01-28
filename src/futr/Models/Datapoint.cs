using static System.Runtime.InteropServices.JavaScript.JSType;

namespace futr.Models;

public class Datapoint : BaseModel
{
    public enum ConfidenceLevel { Canon, Calculated, InformedGuess, CalculatedGuess, WildGuess, Unknown }

    public Civilization Civilization { get; set; }

    public string Metric { get; set; }
    public string Value { get; set; } = "";
    public string Min { get; set; } = "";
    public string Max { get; set; } = "";
    public ConfidenceLevel Confidence { get; set; } = ConfidenceLevel.Unknown;

    public Datapoint(string id, Civilization civilization) : base(id)
    {
        Metric = id;
        Civilization = civilization;
    }

    public new JsonPath.Node fromYaml(string yaml)
    {
        Order = Civilization.Order;

        var node = base.fromYaml(yaml);

        if (Editors.Count == 0) {
            Editors = Civilization.Editors;
        }

        Value = node["value"].AsString.Trim();
        Min = node["min"].AsString.Trim();
        Max = node["max"].AsString.Trim();

        var confidence = node["confidence"].AsString.Trim();
        Confidence = confidence.Trim().ToLower() switch {
            "canon" => ConfidenceLevel.Canon,
            "calculated" => ConfidenceLevel.Calculated,
            "informedguess" => ConfidenceLevel.InformedGuess,
            "informed guess" => ConfidenceLevel.InformedGuess,
            "calculatedguess" => ConfidenceLevel.CalculatedGuess,
            "calculated guess" => ConfidenceLevel.CalculatedGuess,
            "wildguess" => ConfidenceLevel.WildGuess,
            "wild guess" => ConfidenceLevel.WildGuess,
            _ => ConfidenceLevel.Unknown
        };
        
        return node;
    }
}
