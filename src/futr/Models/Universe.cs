namespace futr.Models;

public class Universe
{
    public const string TagSeparator = ",";

    public string? Id { get; set; }
    public string? Name { get; set; }
    public string Description { get; set; } = "";
    public string Tags { get; set; } = "";
}
