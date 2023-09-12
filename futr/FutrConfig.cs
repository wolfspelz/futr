namespace futr;

public class FutrConfig : FutrConfigBag
{
    public bool IsDevelopment = false;
    public string AppName = "FUTR";
    public string ServiceId = "futr";
    public string ClusterId = "local";
    public string AzureTableConnectionString = "UseDevelopmentStorage=true";
    public string CosmosDbConnectionString = "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
}
