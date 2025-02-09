using Microsoft.AspNetCore.Mvc;
using RentACar.Data;
using RentACar.Models;
using System.Linq;

namespace RentACar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var availableCars = _context.Cars.Where(c => c.Quantity > 0).ToList(); // Dohvaća samo dostupne automobile
            return View(availableCars);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
