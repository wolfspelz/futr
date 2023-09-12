using futr.GrainInterfaces;

namespace futr.Pages
{
    public class IndexModel : FutrPageModel
    {
        private readonly IGrainFactory Grains;

        public IndexModel(FutrApp app, IGrainFactory grains) : base(app, "Index")
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
