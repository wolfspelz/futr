using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace futr.Controllers
{
    [Route("proxy")]
    public class ProxyController : FutrControllerBase
    {
        private readonly FileExtensionContentTypeProvider _contentTypeProvider;

        public ProxyController(FutrApp app) : base(app)
        {
            _contentTypeProvider = new FileExtensionContentTypeProvider();
        }

        [HttpGet("{**path}")]
        public IActionResult GetFile(string path)
        {
            if (string.IsNullOrEmpty(path)) {
                return NotFound();
            }

            // Prevent directory traversal attacks
            if (path.Contains("..")) {
                return BadRequest();
            }

            var proxyFolder = Path.Combine(Config.DataFolder, "proxy");
            var filePath = Path.Combine(proxyFolder, path.Replace('/', Path.DirectorySeparatorChar));

            if (!System.IO.File.Exists(filePath)) {
                return NotFound();
            }

            if (!_contentTypeProvider.TryGetContentType(filePath, out var contentType)) {
                contentType = "application/octet-stream";
            }

            var fileStream = System.IO.File.OpenRead(filePath);
            return File(fileStream, contentType);
        }
    }
}
