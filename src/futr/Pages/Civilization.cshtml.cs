using Microsoft.AspNetCore.Mvc;

namespace futr.Pages;

public class CivilizationModel : FutrPageModel
{
    public Models.Civilization? Item { get; private set; }

    public CivilizationModel(FutrApp app) : base(app, nameof(CivilizationModel)) { }

    public IActionResult OnGet(string? id)
    {
        if (id == null) {
            return NotFound();
        } else {
            var item = App.Data.GetCivilization(id);
            if (item == null) {
                return NotFound();
            }
            Item = item;
        }

        return Page();
    }
}
