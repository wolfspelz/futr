namespace futr;

public class FutrConfig : FutrConfigBag
{
    public bool IsDevelopment = false;
    public string LongAppTitle = "Fictional Universe Taxonomy Research - FUTR Project";
    public string AppTitle = "Fictional Universe Taxonomy Research";
    public string AppAcronym = "FUTR";
    public int WebSesssionTimeoutSec = 20 * 60;
    public string DataFolder = "/data";
}
