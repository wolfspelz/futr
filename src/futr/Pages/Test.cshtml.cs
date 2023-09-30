using Microsoft.AspNetCore.Mvc;

namespace futr.Pages;

public class TestModel : FutrPageModel
{
    public string Text = "0";

    public TestModel(FutrApp app) : base(app, nameof(TestModel))
    {
    }

    public IActionResult OnGet(string? id)
    {
        if (id == null) {
            Text = "null";
        } else {
            Text = id;
        }

        return Page();
    }
}
