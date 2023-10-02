using Microsoft.AspNetCore.Mvc;

namespace futr.Pages;

public class MetricModel : FutrPageModel
{
    public string Id { get; private set; } = "";
    public Models.Metric Item { get; private set; } = new Metric("");
    public List<Models.Metric> List { get; private set; } = new();

    public MetricModel(FutrApp app) : base(app, nameof(MetricModel)) { }

    public IActionResult OnGet(string? id)
    {
        if (id == null) {
            List = App.Data.GetMetrics();
        } else {
            Id = id;
            var metric = App.Data.GetMetric(id);
            if (metric == null) {
                return NotFound();
            }
            Item = metric;
        }

        return Page();
    }
}
