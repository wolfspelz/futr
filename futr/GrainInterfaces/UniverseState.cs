namespace futr.GrainInterfaces;

[GenerateSerializer]
public class UniverseState
{
    [Id(0)]
    public string? Id { get; set; }

    [Id(1)]
    public string? Name { get; set; }

    [Id(2)]
    public string? Description { get; set; }

    [Id(3)]
    public string[] Tags { get; set; } = Array.Empty<string>();
}
