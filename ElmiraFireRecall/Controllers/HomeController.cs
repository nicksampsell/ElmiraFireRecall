using ElmiraFireRecall.Data;
using ElmiraFireRecall.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ElmiraFireRecall.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FireDBContext _context;

        public HomeController(ILogger<HomeController> logger, FireDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Authorize(Policy = "AllUsers")]
        public IActionResult Index()
        {
            ViewData["RecipientGroups"] = new SelectList(_context.Groups, "Id", "Title");
            ViewData["EMO_Users"] = new SelectList(_context.Recipients.Include(x => x.FireGroups),"Id","FullName");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}