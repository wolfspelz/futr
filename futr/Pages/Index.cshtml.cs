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

        public UniverseState? Universe { get; private set; }

        public void OnGet()
        {
            //var id = "galdev";

            //var universe = Grains.GetGrain<IUniverseGrain>(id);

            //var firstValue = await universe.Get();

            //await universe.Set(new UniverseState {
            //    Name = "Galactic Developments",
            //    Description = "A universe for testing purposes.",
            //});

            //var secondValue = await universe.Get();

            //Universe = secondValue;
        }
    }
}
