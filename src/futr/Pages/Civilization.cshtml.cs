using Microsoft.AspNetCore.Mvc;

namespace futr.Pages
{
    public class CivilizationModel : FutrPageModel
    {
        public string Id { get; private set; } = "";
        public Models.Civilization? Civilization { get; private set; }

        public CivilizationModel(FutrApp app) : base(app, "Civilization") { }

        public IActionResult OnGet(string? id)
        {
            if (id == null) {
                return NotFound();
            } else {
                Id = id;
                var civilization = App.Data.GetCivilization(id);
                if (civilization == null) {
                    return NotFound();
                }
                Civilization = civilization;
            }

            return Page();
        }
    }
}
