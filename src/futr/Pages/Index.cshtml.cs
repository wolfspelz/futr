namespace futr.Pages
{
    public class IndexModel : FutrPageModel
    {
        public IndexModel(FutrApp app) : base(app, "Index")
        {
        }

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
