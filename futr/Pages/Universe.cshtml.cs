using AutoMapper;
using futr.GrainInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using YamlDotNet.Core.Tokens;

namespace futr.Pages
{
    public class UniverseModel : MyPageModel
    {
        private readonly IGrainFactory _grains;

        public Universe? Universe { get; private set; }

        public UniverseModel(MyApp app, IGrainFactory grains) : base(app, "Universe")
        {
            _grains = grains;
        }

        public async Task OnGet(string id)
        {
            var grain = _grains.GetGrain<IUniverseGrain>(id);
            var state = await grain.Get();
            Universe = App.Mapper.Map<Universe>(state);
        }

        public async Task<IActionResult> OnPost(Universe universe)
        {
            var grain = _grains.GetGrain<IUniverseGrain>(universe.Id);
            var state = App.Mapper.Map<UniverseState>(universe);
            await grain.Set(state);

            //var index = 

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDelete(string id)
        {
            var grain = _grains.GetGrain<IUniverseGrain>(id);
            await grain.Delete();
            return RedirectToPage("./Index");
        }
    }
}
