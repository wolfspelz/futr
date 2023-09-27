using futr.Models;

namespace futr;

public class FutrData
{
    public IDataProvider DataProvider = new TabConvertingFileDataProvider();
    public IDataStructureProvider StructureProvider = new FileStructureProvider();
    public ICallbackLogger Log = new NullCallbackLogger();
    public string MetricsSubfolder = "metrics";
    public string UniversesSubfolder = "universes";
    public string FactionsSubfolder = "_factions";
    public string YamlInfoFileName = "info.yaml";
    public string AlternativeYamlInfoFileName = "info.yml";
    public string ReadmeFileName = "readme.md";

    private Dictionary<string, Metric> _metrics = new();
    private Dictionary<string, Universe> _universes = new();

    public FutrData()
    {
    }

    public void Load(string folderPath)
    {
        Log.Info($"folder={folderPath}");

        LoadMetrics(Path.Combine(folderPath, MetricsSubfolder));
        LoadUniverses(Path.Combine(folderPath, UniversesSubfolder));
    }

    private void LoadMetrics(string folderPath)
    {
        Log.Info($"folder={folderPath}");

        var folders = StructureProvider.EnumerateFolders(folderPath);
        foreach (var path in folders) {
            string metricId = Path.GetFileName(path).Trim();
            var metric = LoadMetric(folderPath, metricId);
            _metrics.Add(metricId, metric);
        }
    }

    private Metric LoadMetric(string folderPath, string metricId)
    {
        var metric = new Metric(metricId);

        var infoPath = Path.Combine(folderPath, metricId, YamlInfoFileName);
        if (!DataProvider.HasData(infoPath)) {
            infoPath = Path.Combine(folderPath, metricId, AlternativeYamlInfoFileName);
        }
        if (DataProvider.HasData(infoPath)) {
            var infoData = DataProvider.GetData(infoPath);
            metric.fromYaml(infoData);
        } else {
            Log.Warning($"Metric {metricId} does not have an info.yaml file.");
        }

        var readmePath = Path.Combine(folderPath, metricId, ReadmeFileName);
        readmePath = FindCaseInsensitiveFile(readmePath);
        if (DataProvider.HasData(readmePath)) {
            var readmeData = DataProvider.GetData(readmePath);
            metric.Description = readmeData;
        }

        return metric;
    }

    private void LoadUniverses(string folderPath)
    {
        Log.Info($"folder={folderPath}");

        var folders = StructureProvider.EnumerateFolders(folderPath);
        foreach (var path in folders) {
            var universeId = Path.GetFileName(path).Trim();
            var universe = LoadUniverse(folderPath, universeId);
            _universes.Add(universeId, universe);
        }
    }

    private Universe LoadUniverse(string folderPath, string universeId)
    {
        var universe = new Universe(universeId);

        var infoPath = Path.Combine(folderPath, universeId, YamlInfoFileName);
        if (!DataProvider.HasData(infoPath)) {
            infoPath = Path.Combine(folderPath, universeId, AlternativeYamlInfoFileName);
        }
        if (DataProvider.HasData(infoPath)) {
            var infoData = DataProvider.GetData(infoPath);
            universe.fromYaml(infoData);
        } else {
            Log.Warning($"Universe {universeId} does not have an info.yaml file.");
        }

        var readmePath = Path.Combine(folderPath, universeId, ReadmeFileName);
        readmePath = FindCaseInsensitiveFile(readmePath);
        if (DataProvider.HasData(readmePath)) {
            var readmeData = DataProvider.GetData(readmePath);
            universe.Description = readmeData;
        }

        var universeFolderPath = Path.Combine(folderPath, universeId);
        var universeSubFolders = StructureProvider.EnumerateFolders(universeFolderPath);
        foreach (var universeSubPath in universeSubFolders) {
            var civilizationId = Path.GetFileName(universeSubPath).Trim();
            if (civilizationId.StartsWith("_")) {
                continue;
            }

            var civilization = LoadCivilization(universeFolderPath, civilizationId);
            universe.Civilizations.Add(civilizationId, civilization);
        }

        var factionFolderPath = Path.Combine(universeFolderPath, FactionsSubfolder);
        var factionSubFolders = StructureProvider.EnumerateFolders(factionFolderPath);
        foreach (var factionSubPath in factionSubFolders) {
            var factionId = Path.GetFileName(factionSubPath).Trim();
            var faction = LoadFaction(factionFolderPath, factionId);
            universe.Factions.Add(factionId, faction);
        }

        return universe;
    }

    private Faction LoadFaction(string folderPath, string factionId)
    {
        var faction = new Faction(factionId);

        var infoPath = Path.Combine(folderPath, factionId, YamlInfoFileName);
        if (!DataProvider.HasData(infoPath)) {
            infoPath = Path.Combine(folderPath, factionId, AlternativeYamlInfoFileName);
        }
        if (DataProvider.HasData(infoPath)) {
            var infoData = DataProvider.GetData(infoPath);
            faction.fromYaml(infoData);
        } else {
            Log.Warning($"Faction {factionId} does not have an info.yaml file.");
        }

        var readmePath = Path.Combine(folderPath, factionId, ReadmeFileName);
        readmePath = FindCaseInsensitiveFile(readmePath);
        if (DataProvider.HasData(readmePath)) {
            var readmeData = DataProvider.GetData(readmePath);
            faction.Description = readmeData;
        }

        return faction;
    }

    private Civilization LoadCivilization(string folderPath, string civilizationId)
    {
        var civilization = new Civilization(civilizationId);

        var civilizationInfoPath = Path.Combine(folderPath, civilizationId, YamlInfoFileName);
        if (!DataProvider.HasData(civilizationInfoPath)) {
            civilizationInfoPath = Path.Combine(folderPath, civilizationId, AlternativeYamlInfoFileName);
        }
        if (DataProvider.HasData(civilizationInfoPath)) {
            var civilizationInfoData = DataProvider.GetData(civilizationInfoPath);
            civilization.fromYaml(civilizationInfoData);
        } else {
            Log.Warning($"Civilization {civilizationId} does not have an info.yaml file.");
        }

        var civilizationReadmePath = Path.Combine(folderPath, civilizationId, ReadmeFileName);
        civilizationReadmePath = FindCaseInsensitiveFile(civilizationReadmePath);
        if (DataProvider.HasData(civilizationReadmePath)) {
            var civilizationReadmeData = DataProvider.GetData(civilizationReadmePath);
            civilization.Description = civilizationReadmeData;
        }

        return civilization;
    }

    public string FindCaseInsensitiveFile(string path)
    {
        string? directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directory)) {
            string lowerFilename = Path.GetFileName(path).ToLower();

            foreach (string filePath in StructureProvider.EnumerateFiles(directory)) {
                var fileName = System.IO.Path.GetFileName(filePath);
                if (fileName.ToLower() == lowerFilename) {
                    return Path.Combine(directory, fileName);
                }
            }
        }

        return path;
    }

    public Universe? GetUniverse(string id)
    {
        return new Universe(id) {
            Title = "Galactic Developments",
            Description = "A universe for testing purposes.",
        };
    }

    public Metric? GetMetric(string id)
    {
        if (_metrics.ContainsKey(id)) {
            return _metrics[id];
        }
        return null;
    }

    public List<Metric> GetMetrics()
    {
        var result = new List<Metric>();

        foreach (var metric in _metrics.Values) {
            result.Add(metric);
        }

        return result;
    }
}
