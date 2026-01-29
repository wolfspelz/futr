using futr;
using futr.Models;
using System.IO;
using System.Reflection;

namespace futr.Test;

[TestClass]
public class FutrDataTest
{
    private FutrData _data = new();
    private static string DataPath => Path.GetFullPath(
        Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
        "..", "..", "..", "..", "..", "data"));

    [TestInitialize]
    public void Setup()
    {
        _data = new FutrData();
        _data.Load(DataPath);
    }

    [TestMethod]
    [TestCategory("futr.Data")]
    public void StarTrek_HasPolities()
    {
        var universe = _data.GetUniverse("Star Trek");
        Assert.IsNotNull(universe);
        Assert.IsTrue(universe.Polities.Count > 0, "Star Trek should have polities");
        Assert.IsTrue(universe.Polities.ContainsKey("Federation of Planets"));
    }

    [TestMethod]
    [TestCategory("futr.Data")]
    public void Polity_HasCorrectSeoName()
    {
        var polity = _data.GetPolity("Star Trek Federation of Planets");
        Assert.IsNotNull(polity);
        Assert.AreEqual("Star Trek Federation of Planets", polity.SeoName);
    }

    [TestMethod]
    [TestCategory("futr.Data")]
    public void Federation2373_HasPolityReference()
    {
        var civ = _data.GetCivilization("Star Trek Federation 2373");
        Assert.IsNotNull(civ);
        Assert.AreEqual("Federation of Planets", civ.Polity);
    }

    [TestMethod]
    [TestCategory("futr.Data")]
    public void GetPolity_ExistingId_ReturnsPolity()
    {
        var polity = _data.GetPolity("Star Trek Federation of Planets");
        Assert.IsNotNull(polity);
        Assert.AreEqual("Federation of Planets", polity.Id);
    }

    [TestMethod]
    [TestCategory("futr.Data")]
    public void GetPolity_NonExistingId_ReturnsNull()
    {
        var polity = _data.GetPolity("NonExistent Polity");
        Assert.IsNull(polity);
    }

    [TestMethod]
    [TestCategory("futr.Data")]
    public void PolitiesSubfolder_HasCorrectValue()
    {
        Assert.AreEqual("_polities", _data.PolitiesSubfolder);
    }
}
