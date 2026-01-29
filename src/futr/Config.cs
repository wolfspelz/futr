namespace futr;

public class Config : FutrConfig
{
    public void Load()
    {
        OverrideConfigFile = CurrentFile;
        ConfigSequence += CurrentFile;

        if (Setup == "dev") {
            IsDevelopment = true;
            DataFolder = "../../data";  // Relative to src/futr/
        }

        AdditionalConfigRoot = System.Environment.GetEnvironmentVariable(ConfigRootEnvironmentVariableName) ?? AdditionalConfigRoot;
        if (!string.IsNullOrEmpty(AdditionalConfigRoot)) {
            BaseFolder = AdditionalConfigRoot;
            Include(OverrideConfigFile);
        }
    }
}
