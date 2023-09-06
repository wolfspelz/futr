using futr.GrainInterfaces;
using Orleans.Runtime;
using System.Runtime.CompilerServices;

namespace futr.Pages
{
    public class IndexModel : MyPageModel
    {
        private readonly IGrainFactory Grains;

        public IndexModel(MyApp app, IGrainFactory grains) : base(app, "Index")
        {
            Grains = grains;
        }

        public Universe? Universe { get; private set; }

        public async Task OnGet()
        {
            Log.Info("");

            var id = "galdev";
            //var universe = Grains.GetGrain<IUniverseGrain>(id).Set(id, new Universe {
            //    Name = "Galactic Developments",
            //    Description = "A universe for testing purposes.",
            //});
            Universe = await Grains.GetGrain<IUniverseGrain>(id).Get();
        }
    }
}
