using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace RentACar.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProcessPayment(string cardNumber, string cvc, string expiryMonth, string expiryYear, string fullName)
        {
            // Provjera da su sva polja popunjena
            if (string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(cvc) ||
                string.IsNullOrEmpty(expiryMonth) || string.IsNullOrEmpty(expiryYear) ||
                string.IsNullOrEmpty(fullName))
            {
                ViewBag.Message = "Sva polja moraju biti popunjena!";
                return View("Index");
            }

            // Validacija: Broj kartice mora imati 16 brojeva
            if (!Regex.IsMatch(cardNumber, @"^\d{16}$"))
            {
                ViewBag.Message = "Broj kartice mora sadržavati točno 16 brojeva!";
                return View("Index");
            }

            // Validacija: CVC mora imati 3 broja
            if (!Regex.IsMatch(cvc, @"^\d{3}$"))
            {
                ViewBag.Message = "CVC mora sadržavati točno 3 broja!";
                return View("Index");
            }

            // Validacija: Mjesec isteka (01-12)
            if (!Regex.IsMatch(expiryMonth, @"^(0[1-9]|1[0-2])$"))
            {
                ViewBag.Message = "Neispravan format mjeseca isteka!";
                return View("Index");
            }

            // Validacija: Godina isteka (dvocifrena)
            if (!Regex.IsMatch(expiryYear, @"^\d{2}$"))
            {
                ViewBag.Message = "Neispravan format godine isteka!";
                return View("Index");
            }

            // Provjera je li datum isteka u budućnosti
            int currentYear = DateTime.Now.Year % 100; // Zadnje dvije cifre trenutne godine
            int currentMonth = DateTime.Now.Month;
            int expMonth = int.Parse(expiryMonth);
            int expYear = int.Parse(expiryYear);

            if (expYear < currentYear || (expYear == currentYear && expMonth < currentMonth))
            {
                ViewBag.Message = "Kartica je istekla!";
                return View("Index");
            }

            // ✅ Dodano logiranje plaćanja (možete povezati s bazom)
            Console.WriteLine($"Plaćanje primljeno: Kartica {cardNumber}, Exp: {expiryMonth}/{expiryYear}, Ime: {fullName}");

            // Plaćanje uspješno
            ViewBag.Message = "Plaćanje uspješno izvršeno!";
            return View("Index");
        }
    }
}
