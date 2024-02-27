using DigitalHub.DataAccess;
using DigitalHub.Models;
using DigitalHub.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHub.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Category> categories = _context.Categories.ToList();
            List<GadgetHomeVM> gadgetVM = new List<GadgetHomeVM>();
            foreach (var category in categories)
            {
                gadgetVM.Add(new GadgetHomeVM
                {
                    CategoryName = category.Name,
                    Gadgets = _context.Gadgets.Where(c => c.Category == category).ToList()
                });
            }
            return View(gadgetVM);
        }
        public IActionResult Authorization()
        {
            return View();
        }
        public IActionResult Registration()
        {
            return View();
        }
    }
}
