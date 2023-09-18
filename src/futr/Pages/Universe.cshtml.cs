using Microsoft.AspNetCore.Mvc;

namespace futr.Pages
{
    public class UniverseModel : FutrPageModel
    {
        public Models.Universe? Universe { get; private set; }

        public UniverseModel(FutrApp app) : base(app, "Universe")
        {
        }

        public void OnGet(string id)
        {
            Universe = App.Data.GetUniverse(id);
        }

        //public async Task<IActionResult> OnPost(Models.Universe universe)
        //{
        //    AssertClaim(FutrRoles.SiteEditor);

        //    var grain = _grains.GetGrain<IUniverseGrain>(universe.Id);
        //    var state = App.Mapper.Map<GrainInterfaces.UniverseState>(universe);
        //    await grain.Set(state);

        //    //var index = 

        //    return RedirectToPage();
        //}

        //public async Task<IActionResult> OnPostDelete(string id)
        //{
        //    AssertClaim("SiteEditor");

        //    var grain = _grains.GetGrain<IUniverseGrain>(id);
        //    await grain.Delete();
        //    return RedirectToPage("./Index");
        //}
    }
}
