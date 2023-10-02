using Microsoft.AspNetCore.Mvc;

namespace futr.Pages;

public class FactionModel : FutrPageModel
{
    public Models.Faction? Item { get; private set; }

    public FactionModel(FutrApp app) : base(app, nameof(FactionModel)) { }

    public IActionResult OnGet(string? id)
    {
        if (id == null) {
            return NotFound();
        } else {
            var item = App.Data.GetFaction(id);
            if (item == null) {
                return NotFound();
            }
            Item = item;
        }

        return Page();
    }
}
