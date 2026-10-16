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
    public void StarTrek_HasPowers()
    {
        var universe = _data.GetUniverse("Star Trek");
        Assert.IsNotNull(universe);
        Assert.IsTrue(universe.Powers.Count > 0, "Star Trek should have powers");
        Assert.IsTrue(universe.Powers.ContainsKey("Federation of Planets"));
    }

    [TestMethod]
    [TestCategory("futr.Data")]
    public void Power_HasCorrectSeoName()
    {
        var power = _data.GetPower("Star Trek Federation of Planets");
        Assert.IsNotNull(power);
        Assert.AreEqual("Star Trek Federation of Planets", power.SeoName);
    }

    [TestMethod]
    [TestCategory("futr.Data")]
    public void Federation2373_HasPowerReference()
    {
        var civ = _data.GetCivilization("Star Trek Federation 2373");
        Assert.IsNotNull(civ);
        Assert.IsNotNull(civ.Power);
        Assert.AreEqual("Federation of Planets", civ.Power.Id);
    }

    [TestMethod]
    [TestCategory("futr.Data")]
    public void GetPower_ExistingId_ReturnsPower()
    {
        var power = _data.GetPower("Star Trek Federation of Planets");
        Assert.IsNotNull(power);
        Assert.AreEqual("Federation of Planets", power.Id);
    }

    [TestMethod]
    [TestCategory("futr.Data")]
    public void GetPower_NonExistingId_ReturnsNull()
    {
        var power = _data.GetPower("NonExistent Power");
        Assert.IsNull(power);
    }

    [TestMethod]
    [TestCategory("futr.Data")]
    public void PowersSubfolder_HasCorrectValue()
    {
        Assert.AreEqual("_powers", _data.PowersSubfolder);
    }
}
