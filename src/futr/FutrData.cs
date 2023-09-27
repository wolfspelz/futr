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

    public void LoadMetrics(string folderPath)
    {
        Log.Info($"folder={folderPath}");

        var folders = StructureProvider.EnumerateFolders(folderPath);
        foreach (var path in folders) {
            string metricId = Path.GetFileName(path).Trim();
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

            _metrics.Add(metricId, metric);
        }
    }

    public void LoadUniverses(string folderPath)
    {
        Log.Info($"folder={folderPath}");

        var folders = StructureProvider.EnumerateFolders(folderPath);
        foreach (var path in folders) {
            var universeId = Path.GetFileName(path).Trim();
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

                var civilization = new Civilization(civilizationId);

                var civilizationInfoPath = Path.Combine(universeFolderPath, civilizationId, YamlInfoFileName);
                if (!DataProvider.HasData(civilizationInfoPath)) {
                    civilizationInfoPath = Path.Combine(universeFolderPath, civilizationId, AlternativeYamlInfoFileName);
                }
                if (DataProvider.HasData(civilizationInfoPath)) {
                    var civilizationInfoData = DataProvider.GetData(civilizationInfoPath);
                    civilization.fromYaml(civilizationInfoData);
                } else {
                    Log.Warning($"Civilization {civilizationId} does not have an info.yaml file.");
                }

                var civilizationReadmePath = Path.Combine(universeFolderPath, civilizationId, ReadmeFileName);
                civilizationReadmePath = FindCaseInsensitiveFile(civilizationReadmePath);
                if (DataProvider.HasData(civilizationReadmePath)) {
                    var civilizationReadmeData = DataProvider.GetData(civilizationReadmePath);
                    civilization.Description = civilizationReadmeData;
                }

                universe.Civilizations.Add(civilizationId, civilization);
            }

            _universes.Add(universeId, universe);
        }
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
