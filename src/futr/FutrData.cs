﻿using futr.Models;

namespace futr;

public class FutrData
{
    public IDataProvider DataProvider = new TabConvertingFileDataProvider();
    public IDataStructureProvider StructureProvider = new FileStructureProvider();
    public ICallbackLogger Log = new NullCallbackLogger();
    public string FolderPath = ".";
    public string MetricsSubfolder = "metrics";
    public string UniversesSubfolder = "universes";
    public string FactionsSubfolder = "_factions";
    public string YamlInfoFileName = "info.yaml";
    public string ReadmeFileName = "readme.md";

    public Dictionary<string, Metric> Metrics = new();
    public Dictionary<string, Universe> Universes = new();
    public Dictionary<string, Civilization> Civilizations = new();
    public Dictionary<string, Faction> Factions = new();

    public FutrData()
    {
    }

    public void Load(string folderPath)
    {
        Log.Info($"{folderPath}");
        FolderPath = folderPath;

        LoadMetrics(Path.Combine(folderPath, MetricsSubfolder));
        LoadUniverses(Path.Combine(folderPath, UniversesSubfolder));

        ExtractCivilizations();
        ExtractFactions();

        CheckUsedMetrics();
    }

    public void Unload()
    {
        Log.Info("");

        Metrics = new();
        Universes = new();
        Civilizations = new();
        Factions = new();
    }

    public void Reload()
    {
        Unload();
        Load(FolderPath);
    }

    private void CheckUsedMetrics()
    {
        foreach (var universe in Universes.Values) {
            foreach (var civilization in universe.Civilizations.Values) {
                foreach(var (datapointId, datapoint) in civilization.Datapoints) {
                    if (!Metrics.ContainsKey(datapoint.Metric)) {
                        Log.Warning($"Metric {datapoint.Metric} is used by {civilization.SeoName} but does not exist.");
                    }
                }
            }
        }
    }

    private void ExtractCivilizations()
    {
        foreach (var universe in Universes.Values) {
            foreach (var civilization in universe.Civilizations.Values) {
                Civilizations.Add(civilization.SeoName, civilization);
            }
        }
    }

    private void ExtractFactions()
    {
        foreach (var universe in Universes.Values) {
            foreach (var faction in universe.Factions.Values) {
                Factions.Add(faction.SeoName, faction);
            }
        }
    }

    private void LoadMetrics(string folderPath)
    {
        Log.Info($"{folderPath}");

        var folders = StructureProvider.EnumerateFolders(folderPath);
        foreach (var path in folders) {
            string metricId = Path.GetFileName(path).Trim();
            var metric = LoadMetric(folderPath, metricId);
            metric.SeoName = metricId;
            Metrics.Add(metricId, metric);
        }
    }

    private Metric LoadMetric(string folderPath, string metricId)
    {
        Log.Info($"{folderPath}/{metricId}");

        var metric = new Metric(metricId);

        var infoPath = FindCaseInsensitiveFile(Path.Combine(folderPath, metricId, YamlInfoFileName));
        if (DataProvider.HasData(infoPath)) {
            var infoData = DataProvider.GetData(infoPath);
            metric.fromYaml(infoData);
        } else {
            Log.Warning($"Metric {metricId} does not have an info.yaml file.");
        }

        var readmePath = FindCaseInsensitiveFile(Path.Combine(folderPath, metricId, ReadmeFileName));
        if (DataProvider.HasData(readmePath)) {
            var readmeData = DataProvider.GetData(readmePath);
            metric.Readme = readmeData;
        }

        return metric;
    }

    private void LoadUniverses(string folderPath)
    {
        Log.Info($"{folderPath}");

        var folders = StructureProvider.EnumerateFolders(folderPath);
        foreach (var path in folders) {
            var universeId = Path.GetFileName(path).Trim();
            var universe = LoadUniverse(folderPath, universeId);
            universe.SeoName = universeId;
            Universes.Add(universeId, universe);
        }
    }

    private Universe LoadUniverse(string folderPath, string universeId)
    {
        Log.Info($"{folderPath}/{universeId}");
        var universe = new Universe(universeId);

        var infoPath = FindCaseInsensitiveFile(Path.Combine(folderPath, universeId, YamlInfoFileName));
        if (DataProvider.HasData(infoPath)) {
            var infoData = DataProvider.GetData(infoPath);
            universe.fromYaml(infoData);
        } else {
            Log.Warning($"Universe {universeId} does not have an info.yaml file.");
        }

        var readmePath = FindCaseInsensitiveFile(Path.Combine(folderPath, universeId, ReadmeFileName));
        if (DataProvider.HasData(readmePath)) {
            var readmeData = DataProvider.GetData(readmePath);
            universe.Readme = readmeData;
        }

        var universeFolderPath = Path.Combine(folderPath, universeId);
        var universeSubFolders = StructureProvider.EnumerateFolders(universeFolderPath);
        foreach (var universeSubPath in universeSubFolders) {
            var civilizationId = Path.GetFileName(universeSubPath).Trim();
            if (civilizationId.StartsWith("_")) {
                continue;
            }

            var civilization = LoadCivilization(universe, universeFolderPath, civilizationId);
            if (civilization != null) {
                civilization.SeoName = $"{universeId} {civilizationId}";
                universe.Civilizations.Add(civilizationId, civilization);
            }
        }

        var factionFolderPath = Path.Combine(universeFolderPath, FactionsSubfolder);
        var factionSubFolders = StructureProvider.EnumerateFolders(factionFolderPath);
        foreach (var factionSubPath in factionSubFolders) {
            var factionId = Path.GetFileName(factionSubPath).Trim();
            var faction = LoadFaction(universe, factionFolderPath, factionId);
            if (faction != null) {
                faction.SeoName = $"{universeId} {factionId}";
                universe.Factions.Add(factionId, faction);
            }
        }

        return universe;
    }

    private Faction LoadFaction(Universe universe, string folderPath, string factionId)
    {
        Log.Info($"{folderPath}/{factionId}");
        var faction = new Faction(factionId, universe);

        var infoPath = FindCaseInsensitiveFile(Path.Combine(folderPath, factionId, YamlInfoFileName));
        if (DataProvider.HasData(infoPath)) {
            var infoData = DataProvider.GetData(infoPath);
            faction.fromYaml(infoData);
        } else {
            Log.Warning($"Faction {factionId} does not have an info.yaml file.");
        }

        var readmePath = FindCaseInsensitiveFile(Path.Combine(folderPath, factionId, ReadmeFileName));
        if (DataProvider.HasData(readmePath)) {
            var readmeData = DataProvider.GetData(readmePath);
            faction.Readme = readmeData;
        }

        return faction;
    }

    private Civilization LoadCivilization(Universe universe, string folderPath, string civilizationId)
    {
        Log.Info($"{folderPath}/{civilizationId}");
        var civilization = new Civilization(civilizationId, universe);

        var civilizationInfoPath = FindCaseInsensitiveFile(Path.Combine(folderPath, civilizationId, YamlInfoFileName));
        if (DataProvider.HasData(civilizationInfoPath)) {
            var civilizationInfoData = DataProvider.GetData(civilizationInfoPath);
            civilization.fromYaml(civilizationInfoData);
        } else {
            Log.Warning($"Civilization {civilizationId} does not have an info.yaml file. Ignoring.");
        }

        var civilizationReadmePath = FindCaseInsensitiveFile(Path.Combine(folderPath, civilizationId, ReadmeFileName));
        if (DataProvider.HasData(civilizationReadmePath)) {
            var civilizationReadmeData = DataProvider.GetData(civilizationReadmePath);
            civilization.Readme = civilizationReadmeData;
        }

        {
            var civilizationFolderPath = Path.Combine(folderPath, civilizationId);
            var civilizationSubFolders = StructureProvider.EnumerateFolders(civilizationFolderPath);
            foreach (var civilizationSubPath in civilizationSubFolders) {
                var datapointId = Path.GetFileName(civilizationSubPath).Trim();
                if (datapointId.StartsWith("_")) {
                    continue;
                }

                var datapoint = LoadDatapoint(civilization, civilizationFolderPath, datapointId);
                if (datapoint != null) {
                    datapoint.SeoName = $"{universe.Id} {civilizationId} {datapointId}";
                    civilization.Datapoints.Add(datapointId, datapoint);
                }
            }

        }

        return civilization;
    }

    private Datapoint LoadDatapoint(Civilization civilization, string folderPath, string datapointId)
    {
        Log.Info($"{folderPath}/{datapointId}");
        var datapoint = new Datapoint(datapointId, civilization);

        var infoPath = FindCaseInsensitiveFile(Path.Combine(folderPath, datapointId, YamlInfoFileName));
        if (DataProvider.HasData(infoPath)) {
            var infoData = DataProvider.GetData(infoPath);
            datapoint.fromYaml(infoData);
        } else {
            Log.Warning($"Datapoint {datapointId} does not have an info.yaml file.");
        }

        var readmePath = FindCaseInsensitiveFile(Path.Combine(folderPath, datapointId, ReadmeFileName));
        if (DataProvider.HasData(readmePath)) {
            var readmeData = DataProvider.GetData(readmePath);
            datapoint.Readme = readmeData;
        }

        return datapoint;
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

    public Metric? GetMetric(string id)
    {
        if (Metrics.ContainsKey(id)) {
            return Metrics[id];
        }
        return null;
    }

    public Universe? GetUniverse(string id)
    {
        if (Universes.ContainsKey(id)) {
            return Universes[id];
        }
        return null;
    }

    public Faction? GetFaction(string id)
    {
        if (Factions.ContainsKey(id)) {
            return Factions[id];
        }
        return null;
    }

    public Civilization? GetCivilization(string id)
    {
        if (Civilizations.ContainsKey(id)) {
            return Civilizations[id];
        }
        return null;
    }

    public List<Metric> GetMetrics()
    {
        var result = new List<Metric>();

        foreach (var metric in Metrics.Values) {
            result.Add(metric);
        }

        return result;
    }

    public List<Universe> GetUniverses()
    {
        var result = new List<Universe>();

        foreach (var universe in Universes.Values) {
            result.Add(universe);
        }

        return result;
    }

}
