namespace futr.Models;

public class Datapoint
{
    public enum ConfidenceLevel { Canon, Calculated, InformedGuess, CalculatedGuess, WildGuess, Unknown }

    public string Metric { get; set; }
    public string Value { get; set; }
    public string Min { get; set; }
    public string Max { get; set; }
    public ConfidenceLevel Confidence { get; set; }
    public string Comment { get; set; } = "";
    public List<ReferenceModel> References = new();
    public string Error { get; set; } = "";

    public Datapoint(string metric, string value, string min, string max, string confidence)
    {
        Metric = metric;
        Value = value;
        Min = min;
        Max = max;
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
        if (Confidence == ConfidenceLevel.Unknown) {
            Error += $"[Unknown confidence level: {confidence}]";
        }
    }
}
