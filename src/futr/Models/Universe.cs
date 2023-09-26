namespace futr.Models;

public class Universe
{
    public const string TagSeparator = ",";

    public string Id { get; set; }
    public string Title { get; set; }
    public string Tags { get; set; } = "";
    public string Description { get; set; } = "";

    public Universe(string id)
    {
        Id = id;
        Title = id;
    }
}
