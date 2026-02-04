using futr.Models;

namespace futr.Test;

[TestClass]
public class BaseModelTest
{
    [TestMethod]
    [TestCategory("futr.Models")]
    public void Order_MissingInYaml_DefaultsToZero()
    {
        var yaml = @"
title: Test Item
tags: [index]
";
        var model = new Universe("test");
        model.fromYaml(yaml);

        Assert.AreEqual(0.0, model.Order, "Missing order should default to 0");
    }

    [TestMethod]
    [TestCategory("futr.Models")]
    public void Order_PresentInYaml_ParsedCorrectly()
    {
        var yaml = @"
title: Test Item
order: 42.5
tags: [index]
";
        var model = new Universe("test");
        model.fromYaml(yaml);

        Assert.AreEqual(42.5, model.Order, "Order should be parsed from YAML");
    }

    [TestMethod]
    [TestCategory("futr.Models")]
    public void Order_NegativeValue_ParsedCorrectly()
    {
        var yaml = @"
title: Test Item
order: -1000
tags: [index]
";
        var model = new Universe("test");
        model.fromYaml(yaml);

        Assert.AreEqual(-1000.0, model.Order, "Negative order should be parsed correctly");
    }

    [TestMethod]
    [TestCategory("futr.Models")]
    public void Created_MissingInYaml_DefaultsToMinValue()
    {
        var yaml = @"
title: Test Item
";
        var model = new Universe("test");
        model.fromYaml(yaml);

        Assert.AreEqual(DateTime.MinValue, model.Created, "Missing created should default to MinValue");
    }

    [TestMethod]
    [TestCategory("futr.Models")]
    public void Created_PresentInYaml_ParsedCorrectly()
    {
        var yaml = @"
title: Test Item
created: 2026-02-04
";
        var model = new Universe("test");
        model.fromYaml(yaml);

        Assert.AreEqual(new DateTime(2026, 2, 4), model.Created, "Created date should be parsed from YAML");
    }

    [TestMethod]
    [TestCategory("futr.Models")]
    public void Changed_PresentInYaml_ParsedCorrectly()
    {
        var yaml = @"
title: Test Item
changed: 2026-01-15
";
        var model = new Universe("test");
        model.fromYaml(yaml);

        Assert.AreEqual(new DateTime(2026, 1, 15), model.Changed, "Changed date should be parsed from YAML");
    }
}
