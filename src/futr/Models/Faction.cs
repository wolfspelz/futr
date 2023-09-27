﻿namespace futr.Models;

public class Faction : BaseModel
{
    public Universe Universe { get; set; }

    //public string Type { get; set; } = "";

    public Faction(Universe universe, string id) : base(id)
    {
        Universe = universe;
    }

    public new JsonPath.Node fromYaml(string yaml)
    {
        var node = base.fromYaml(yaml);

        //Type = node["type"].AsString.Trim();

        return node;
    }
}