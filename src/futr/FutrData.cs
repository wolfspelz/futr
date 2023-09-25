namespace futr;

public class FutrData
{
    public IDataProvider DataProvider = new TabConvertingFileDataProvider();
    public IStructureProvider StructureProvider = new FileStructureProvider();
    public ICallbackLogger Log = new NullCallbackLogger();
    public string MetricsSubfolder = "metrics";
    public string UniversesSubfolder = "universes";
    public string FactionsSubfolder = "_factions";
    public string InfoFileName = "info.yaml";

    public FutrData()
    {
    }

    public void Load(string folderPath)
    {
        Log.Info($"folder={folderPath}");
        LoadMetrics(Path.Combine(folderPath, MetricsSubfolder));
        LoadUniverses(Path.Combine(folderPath, UniversesSubfolder));
    }

    public void LoadMetrics(string folderPath)
    {
        Log.Info($"folder={folderPath}");
        var metrics = StructureProvider.EnumerateFolders(folderPath);
        foreach (var metricId in metrics) {
            var metricInfoPath = Path.Combine(folderPath, metricId, InfoFileName);
            var metricData = DataProvider.GetData(metricInfoPath);
        }
    }

    public void LoadUniverses(string folderPath)
    {
        Log.Info($"folder={folderPath}");
    }

    public Universe? GetUniverse(string id)
    {
        return new Universe {
            Id = id,
            Name = "Galactic Developments",
            Description = "A universe for testing purposes.",
        };
    }
}