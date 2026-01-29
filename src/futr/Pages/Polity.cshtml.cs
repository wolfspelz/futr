using Microsoft.AspNetCore.Mvc;

namespace futr.Pages;

public class PolityModel : FutrPageModel
{
    public Models.Polity? Item { get; private set; }

    public PolityModel(FutrApp app) : base(app, nameof(PolityModel)) { }

    public IActionResult OnGet(string? id)
    {
        if (id == null) {
            return NotFound();
        } else {
            var item = App.Data.GetPolity(id);
            if (item == null) {
                return NotFound();
            }
            Item = item;
        }

        return Page();
    }
}
