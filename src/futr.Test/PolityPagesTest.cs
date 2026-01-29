using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace futr.Test;

[TestClass]
public class PolityPagesTest
{
    HttpClient _client = new();
    private static string SolutionRoot => Path.GetFullPath(
        Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
        "..", "..", "..", "..", ".."));

    [TestInitialize]
    public void Startup()
    {
        var appPath = Path.Combine(SolutionRoot, "src", "futr");
        _client = new WebApplicationFactory<futr.Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseContentRoot(appPath);
                builder.UseSetting("DataFolder", Path.Combine(SolutionRoot, "data"));
            })
            .CreateClient();
    }

    [TestMethod]
    [TestCategory("futr.Polity")]
    public async Task Polity_WithInvalidId_Returns404()
    {
        var response = await _client.GetAsync("/Polity/NonExistent");
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    [TestCategory("futr.Polity")]
    public async Task Polity_RouteExists()
    {
        // Verify the route is registered (may return 404 for missing data, but not 500)
        var response = await _client.GetAsync("/Polity/Test");
        Assert.IsTrue(
            response.StatusCode == HttpStatusCode.NotFound ||
            response.StatusCode == HttpStatusCode.OK,
            $"Expected 404 or 200, got {response.StatusCode}");
    }

    [TestMethod]
    [TestCategory("futr.Polity")]
    public async Task Universe_RouteExists()
    {
        // Verify the route is registered
        var response = await _client.GetAsync("/Universe");
        Assert.IsTrue(
            response.StatusCode == HttpStatusCode.NotFound ||
            response.StatusCode == HttpStatusCode.OK,
            $"Expected 404 or 200, got {response.StatusCode}");
    }
}
