using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Controllers
{
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Prikazuje sve automobile s opcijom filtriranja i pretrage
        public async Task<IActionResult> Index(bool? availableOnly, string searchTerm)
        {
            var carsQuery = _context.Cars.Include(c => c.Reservations);
            var cars = await carsQuery.ToListAsync(); // Dohvati sve automobile

            // ✅ Filtriranje dostupnih automobila nakon dohvaćanja podataka
            if (availableOnly.HasValue && availableOnly.Value)
            {
                cars = cars.Where(c => c.AvailableQuantity > 0).ToList();
            }

            // ✅ Pretraga automobila po korisničkom unosu (ignorira velika/mala slova)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                cars = cars.Where(c =>
                    c.Brand.ToLower().Contains(searchTerm) ||
                    c.Model.ToLower().Contains(searchTerm) ||
                    c.Fuel.ToLower().Contains(searchTerm) ||
                    c.Transmission.ToLower().Contains(searchTerm)).ToList();
            }

            // ✅ Ako nema rezultata pretrage, prikaži poruku
            ViewBag.NoCarsFound = cars.Any() ? null : "Ovaj auto ne postoji ili nije dostupan.";

            return View(cars);
        }

        // ✅ Dodavanje novog automobila - samo admin može dodavati
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddCar([FromBody] Car car)
        {
            if (car == null)
            {
                return BadRequest(new { success = false, message = "Podaci nisu ispravni." });
            }

            // ✅ Provjera validacije modela prije dodavanja
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Unos nije validan. Provjerite polja." });
            }

            // ✅ Osiguranje da Quantity ima vrijednost
            if (car.Quantity <= 0)
            {
                return BadRequest(new { success = false, message = "Količina mora biti veća od 0." });
            }

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }

        // ✅ Prikaz forme za rezervaciju automobila
        public async Task<IActionResult> Reserve(int id)
        {
            var car = await _context.Cars.Include(c => c.Reservations).FirstOrDefaultAsync(c => c.Id == id);
            if (car == null || car.AvailableQuantity <= 0)
            {
                return NotFound("Automobil nije dostupan za rezervaciju.");
            }

            var reservation = new Reservation
            {
                CarId = id,
                StartDate = System.DateTime.Now.AddDays(1),
                EndDate = System.DateTime.Now.AddDays(4),
                Status = ReservationStatus.Pending
            };

            return View("Reserve", reservation);
        }

        // ✅ Potvrda rezervacije
        [HttpPost]
        public async Task<IActionResult> ReserveConfirm(int id, System.DateTime startDate, System.DateTime endDate)
        {
            var car = await _context.Cars.Include(c => c.Reservations).FirstOrDefaultAsync(c => c.Id == id);
            if (car == null || car.AvailableQuantity <= 0)
            {
                return NotFound("Automobil nije dostupan za rezervaciju.");
            }

            var reservation = new Reservation
            {
                CarId = id,
                StartDate = startDate,
                EndDate = endDate,
                Status = ReservationStatus.Confirmed
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ✅ Otkazivanje rezervacije
        [HttpPost]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound("Rezervacija nije pronađena.");
            }

            // Otkazujemo rezervaciju
            reservation.Status = ReservationStatus.Canceled;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
