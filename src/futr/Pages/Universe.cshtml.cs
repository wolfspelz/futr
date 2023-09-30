using Microsoft.AspNetCore.Mvc;

namespace futr.Pages;

public class UniverseModel : FutrPageModel
{
    public string Id { get; private set; } = "";
    public Models.Universe Universe { get; private set; } = new Universe("");
    public List<Models.Universe> Universes { get; private set; } = new();

    public UniverseModel(FutrApp app) : base(app, nameof(UniverseModel)) { }

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
