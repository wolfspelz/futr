using Microsoft.AspNetCore.Mvc;

namespace futr.Pages;

public class TestFModel : FutrPageModel
{
    public string Id { get; private set; } = "";
    public Models.Faction? Faction { get; private set; }

    public TestFModel(FutrApp app) : base(app, nameof(TestFModel)) { }

    public IActionResult OnGet(string? id)
    {
        if (id == null) {
            return NotFound();
        } else {
            Id = id;
            var faction = App.Data.GetFaction(id);
            if (faction == null) {
                return NotFound();
            }
            Faction = faction;
        }

        return Page();
    }
}
