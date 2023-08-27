namespace futr.Models;

[GenerateSerializer]
public class Universe
{
    [Id(0)]
    public string? Name { get; set; }

    [Id(1)]
    public string? Description { get; set; }
}
