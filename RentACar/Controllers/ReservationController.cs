using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ✅ PRIKAZ REZERVACIJA (Korisnik vidi samo svoje, admin vidi sve)
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var userReservations = await _context.Reservations
                .Include(r => r.Car)
                .Where(r => r.UserId == user.Id)
                .ToListAsync();

            var allReservations = User.IsInRole("Admin")
                ? await _context.Reservations
                    .Include(r => r.Car)
                    .Include(r => r.User)
                    .ToListAsync()
                : new List<Reservation>();

            // ✅ Dohvati sve automobile
            var allCars = await _context.Cars.ToListAsync();

            // ✅ Izračunaj dostupnu količinu u memoriji BEZ promjene originalnih entiteta
            var availableCars = allCars
                .Select(car => new
                {
                    Car = car,
                    AvailableQuantity = car.Quantity - _context.Reservations
                        .Count(r => r.CarId == car.Id && r.Status == ReservationStatus.Confirmed)
                })
                .Where(c => c.AvailableQuantity > 0) // ✅ Filtriraj samo dostupne automobile
                .Select(c => c.Car) // ✅ Samo entiteti automobila
                .ToList();

            var viewModel = new ReservationViewModel
            {
                UserReservations = userReservations,
                AllReservations = allReservations,
                AvailableCars = availableCars
            };

            return View(viewModel);
        }

        // ✅ KREIRANJE REZERVACIJE
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(int carId, DateTime startDate, DateTime endDate)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            if (startDate >= endDate)
            {
                ModelState.AddModelError("", "Datum završetka mora biti nakon početnog datuma.");
                return RedirectToAction("Index");
            }

            var car = await _context.Cars.FindAsync(carId);
            if (car == null)
            {
                ModelState.AddModelError("", "Odabrani automobil ne postoji.");
                return RedirectToAction("Index");
            }

            // ✅ Provjeri dostupnost automobila
            int reservedCount = await _context.Reservations
                .CountAsync(r => r.CarId == carId && r.Status == ReservationStatus.Confirmed);

            if (reservedCount >= car.Quantity)
            {
                ModelState.AddModelError("", "Odabrani automobil nije dostupan.");
                return RedirectToAction("Index");
            }

            var reservation = new Reservation
            {
                CarId = carId,
                UserId = user.Id ?? string.Empty, // Ako je UserId null, koristimo prazan string
                StartDate = startDate,
                EndDate = endDate,
                Status = ReservationStatus.Pending // ✅ Početni status: "Na čekanju"
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // ✅ OTKAZIVANJE REZERVACIJE (Samo korisnik može otkazati svoju rezervaciju)
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == user.Id);

            if (reservation == null)
            {
                return NotFound("Rezervacija nije pronađena ili nemate ovlasti za otkazivanje.");
            }

            reservation.Status = ReservationStatus.Canceled;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // ✅ ADMIN MOŽE PRIHVATITI ILI ODBITI REZERVACIJU
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Confirm(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound("Rezervacija nije pronađena.");
            }

            reservation.Status = ReservationStatus.Confirmed;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound("Rezervacija nije pronađena.");
            }

            reservation.Status = ReservationStatus.Rejected; // ODBIJENA REZERVACIJA
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
