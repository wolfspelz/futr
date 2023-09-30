using Microsoft.AspNetCore.Mvc;

namespace futr.Pages;

public class TestModel : FutrPageModel
{
    public string Id = "_notset";
    public Models.Universe Universe { get; private set; } = new Universe("");
    public List<Models.Universe> Universes { get; private set; } = new();

    public TestModel(FutrApp app) : base(app, nameof(TestModel))
    {
    }

    public IActionResult OnGet(string? id)
    {
        if (id == null) {
            Id = "";
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
