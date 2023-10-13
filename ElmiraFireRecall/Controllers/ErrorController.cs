using ElmiraFireRecall.Data;
using Microsoft.AspNetCore.Mvc;

namespace ElmiraFireRecall.Controllers
{
    [Route("error")]
    public class ErrorsController : Controller
    {

        private readonly FireDBContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;


        public ErrorsController(FireDBContext context,
            IWebHostEnvironment env,
            IConfiguration config)
        {
            _context = context;
            _env = env;
            _config = config;
        }

        [Route("500")]
        public async Task<IActionResult> AppError()
        {

            return View();
        }

        [Route("404")]
        public async Task<IActionResult> PageNotFound()
        {
            return View();
        }

        [Route("403")]
        public async Task<IActionResult> NoPermission()
        {
            return View();
        }

    }
}
