namespace futr;

public class FutrConfigBag : ConfigSharp.ConfigBag
{
    public string Setup =
#if DEBUG
        "dev";
#else
        "prod";
#endif

    public string ConfigSequence = "";
    public string OverrideConfigFile = "OverrideConfig.cs";
    public string ConfigRootEnvironmentVariableName = "FUTR_CONFIG_ROOT";
    public string AdditionalConfigRoot = "";
    public string LogLevel = "";
}
