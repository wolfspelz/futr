using futr.GrainInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace futr.Pages
{
    public class UniverseModel : FutrPageModel
    {
        private readonly IGrainFactory _grains;

        public Models.Universe? Universe { get; private set; }

        public UniverseModel(FutrApp app, IGrainFactory grains) : base(app, "Universe")
        {
            _grains = grains;
        }

        public async Task OnGet(string id)
        {
            var grain = _grains.GetGrain<IUniverseGrain>(id);
            var state = await grain.Get();
            Universe = App.Mapper.Map<Models.Universe>(state);
        }

        public async Task<IActionResult> OnPost(Models.Universe universe)
        {
            AssertClaim(FutrRoles.SiteEditor);

            var grain = _grains.GetGrain<IUniverseGrain>(universe.Id);
            var state = App.Mapper.Map<GrainInterfaces.UniverseState>(universe);
            await grain.Set(state);

            //var index = 

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDelete(string id)
        {
            AssertClaim("SiteEditor");

            var grain = _grains.GetGrain<IUniverseGrain>(id);
            await grain.Delete();
            return RedirectToPage("./Index");
        }
    }
}
