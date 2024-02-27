using DigitalHub.DataAccess;
using DigitalHub.Models;
using DigitalHub.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DigitalHub.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GadgetController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public GadgetController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Gadget> gadgets = _context.Gadgets.Include(c => c.Category).ToList();
            return View(gadgets);
        }
        public IActionResult Create()
        {
            GadgetVM gadgetVM = new()
            {
                CategoryList = _context.Categories.ToList().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                Gadget = new Gadget()
            };
            return View(gadgetVM);
        }
        [HttpPost]
        public IActionResult Create(GadgetVM gadgetVM, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string gadgetPath = Path.Combine(wwwRootPath, @"images\gadget");

                using (var fileStream = new FileStream(Path.Combine(gadgetPath, fileName), FileMode.Create))
                    file.CopyTo(fileStream);

                gadgetVM.Gadget.ImageUrl = @"\images\gadget\" + fileName;

                _context.Gadgets.Add(gadgetVM.Gadget);
                _context.SaveChanges();
                TempData["success"] = "Товар успешно добавлен";
                return RedirectToAction("Index");
            }
            return View();
        }
    
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            GadgetVM gadgetVM = new()
            {
                CategoryList = _context.Categories.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Gadget = _context.Gadgets.FirstOrDefault(x => x.Id == id)
            };
            return View(gadgetVM);
        }
        [HttpPost]
        public IActionResult Edit(GadgetVM gadgetVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string gadgetPath = Path.Combine(wwwRootPath, @"images\gadget");
                    //remove old image
                    if (!string.IsNullOrEmpty(gadgetVM.Gadget.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, gadgetVM.Gadget.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
                    }

                    using (var fileStream = new FileStream(Path.Combine(gadgetPath, fileName), FileMode.Create))
                        file.CopyTo(fileStream);

                    gadgetVM.Gadget.ImageUrl = @"\images\gadget\" + fileName;
                }
                _context.Gadgets.Update(gadgetVM.Gadget);
                _context.SaveChanges();
                TempData["success"] = "Товар успешно обновлен";
                return RedirectToAction("Index");
            }
            return View();
        }
    
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Gadget? gadget = _context.Gadgets.FirstOrDefault(x => x.Id == id);
            if (gadget == null) return NotFound();
            return View(gadget);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Gadget? gadget = _context.Gadgets.FirstOrDefault(x => x.Id == id);

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            var imagePath = Path.Combine(wwwRootPath, gadget.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);

            _context.Gadgets.Remove(gadget);
            _context.SaveChanges();
            TempData["success"] = "Товар успешно удален";
            return RedirectToAction("Index");
        }
    }
}
