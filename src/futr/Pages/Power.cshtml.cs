using Microsoft.AspNetCore.Mvc;

namespace futr.Pages;

public class PowerModel : FutrPageModel
{
    public Models.Power? Item { get; private set; }

    public PowerModel(FutrApp app) : base(app, nameof(PowerModel)) { }

    public IActionResult OnGet(string? id)
    {
        if (id == null) {
            return NotFound();
        } else {
            var item = App.Data.GetPower(id);
            if (item == null) {
                return NotFound();
            }
            Item = item;
        }

        return Page();
    }
}
