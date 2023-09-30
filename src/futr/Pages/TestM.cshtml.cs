using Microsoft.AspNetCore.Mvc;

namespace futr.Pages;

public class TestMModel : FutrPageModel
{
    public string Id { get; private set; } = "";
    public Models.Metric Metric { get; private set; } = new Metric("");
    public List<Models.Metric> Metrics { get; private set; } = new();

    public TestMModel(FutrApp app) : base(app, nameof(TestMModel)) { }

    public IActionResult OnGet(string? id)
    {
        if (id == null) {
            Metrics = App.Data.GetMetrics();
        } else {
            Id = id;
            var metric = App.Data.GetMetric(id);
            if (metric == null) {
                return NotFound();
            }
            Metric = metric;
        }

        return Page();
    }
}
