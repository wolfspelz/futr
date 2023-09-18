namespace futr;

public class FutrData
{
    public FutrData()
    {
    }

    internal Universe? GetUniverse(string id)
    {
       return new Universe {
           Id = id,
           Name = "Galactic Developments",
           Description = "A universe for testing purposes.",
       };
    }
}