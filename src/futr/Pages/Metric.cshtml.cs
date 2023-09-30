using Microsoft.AspNetCore.Mvc;

namespace futr.Pages;

public class MetricModel : FutrPageModel
{
    public string Id { get; private set; } = "";
    public Models.Metric Metric { get; private set; } = new Metric("");
    public List<Models.Metric> Metrics { get; private set; } = new();

    public MetricModel(FutrApp app) : base(app, nameof(MetricModel)) { }

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
