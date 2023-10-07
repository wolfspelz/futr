using Microsoft.AspNetCore.Mvc;

namespace futr.Controllers
{
    public class ReloadController : FutrControllerBase
    {
        public ReloadController(FutrApp app) : base(app) { }

        [HttpGet]
        public IActionResult Index()
        {
            App.Data.Reload();

            return new ContentResult {
                Content = "ok"
            };
        }
    }
}
