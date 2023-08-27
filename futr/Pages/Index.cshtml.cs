namespace futr.Pages
{
    public class IndexModel : MyPageModel
    {
        public IndexModel(MyApp app) : base(app, "Index")
        {
        }

        public void OnGet()
        {
            Log.Info("");
        }
    }
}