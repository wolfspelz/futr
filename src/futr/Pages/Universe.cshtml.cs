﻿using Microsoft.AspNetCore.Mvc;

namespace futr.Pages;

public class UniverseModel : FutrPageModel
{
    public string Id { get; private set; } = "";
    public Models.Universe Item { get; private set; } = new Universe("");
    public List<Models.Universe> List { get; private set; } = new();

    public UniverseModel(FutrApp app) : base(app, nameof(UniverseModel)) { }

    public IActionResult OnGet(string? id)
    {
        if (id == null) {
            List = App.Data.GetUniverses();
        } else {
            if (IdSuggestsImage(id)) {
                return Redirect("/image" + HttpContext.Request.Path);
            }
            Id = id;
            var universe = App.Data.GetUniverse(id);
            if (universe == null) {
                return NotFound();
            }
            Item = universe;
        }

        return Page();
    }

    private bool IdSuggestsImage(string id)
    {
        return id.EndsWith(".png") || id.EndsWith(".jpg") || id.EndsWith(".jpeg");
    }
}
