using Microsoft.AspNetCore.Mvc;

namespace futr.Pages;

public class TestUModel : FutrPageModel
{
    public string Id { get; private set; } = "";
    public Models.Universe Universe { get; private set; } = new Universe("");
    public List<Models.Universe> Universes { get; private set; } = new();

    public TestUModel(FutrApp app) : base(app, nameof(TestUModel)) { }

    public IActionResult OnGet(string? id)
    {
        if (id == null) {
            Universes = App.Data.GetUniverses();
        } else {
            Id = id;
            var universe = App.Data.GetUniverse(id);
            if (universe == null) {
                return NotFound();
            }
            Universe = universe;
        }

        return Page();
    }
}
