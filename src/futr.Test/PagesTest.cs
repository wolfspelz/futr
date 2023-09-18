using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Threading.Tasks;

namespace futr.Test;

[TestClass]
public class PagesTest
{
    HttpClient _client = new();

    [TestInitialize]
    public void Startup()
    {
        _client = new WebApplicationFactory<futr.Program>()
            //.WithWebHostBuilder(builder => builder.UseSolutionRelativeContentRoot("code/galdevweb/GaldevWeb"))
            .CreateClient();
    }

    [TestMethod]
    [TestCategory("futr.Pages")]
    public async Task Index_works_and_has_charset()
    {
        // Act
        var response = await _client.GetAsync("/Index?lang=en-US");

        // Assert
        Assert.IsNotNull(response.Content);
        response.EnsureSuccessStatusCode();
        Assert.AreEqual("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        var doc = new HtmlDocument();
        doc.LoadHtml(await response.Content.ReadAsStringAsync());
        Assert.AreEqual("utf-8", doc.DocumentNode.SelectNodes("//meta").FirstOrDefault(n => n.GetAttributes("charset").Count() > 0)?.Attributes["charset"].Value);
    }

    [TestMethod]
    [TestCategory("futr.Pages")]
    public async Task Index_with_forced_lang_de()
    {
        // Act
        var response = await _client.GetAsync("/Index?lang=de-DE");

        // Assert
        Assert.IsNotNull(response.Content);
        response.EnsureSuccessStatusCode();
        Assert.AreEqual("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        var doc = new HtmlDocument();
        doc.LoadHtml(await response.Content.ReadAsStringAsync());
        Assert.AreEqual("utf-8", doc.DocumentNode.SelectNodes("//meta").FirstOrDefault(n => n.GetAttributes("charset").Count() > 0)?.Attributes["charset"].Value);
    }

    [TestMethod]
    [TestCategory("futr.Pages")]
    public async Task Privacy_returns_OK()
    {
        // Act
        var response = await _client.GetAsync("/Privacy");

        // Assert
        Assert.IsNotNull(response.Content);
        response.EnsureSuccessStatusCode();
        Assert.AreEqual("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
    }

}
