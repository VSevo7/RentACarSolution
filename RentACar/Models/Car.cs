using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace RentACar.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Marka automobila je obavezna.")]
        [StringLength(50, ErrorMessage = "Naziv marke ne može biti duži od 50 znakova.")]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model automobila je obavezan.")]
        [StringLength(50, ErrorMessage = "Naziv modela ne može biti duži od 50 znakova.")]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vrsta mjenjača je obavezna.")]
        public string Transmission { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vrsta goriva je obavezna.")]
        public string Fuel { get; set; } = string.Empty;

        [Required(ErrorMessage = "Količina vozila je obavezna.")]
        [Range(1, 100, ErrorMessage = "Količina mora biti između 1 i 100.")]
        public int Quantity { get; set; }

        public bool IsReserved { get; set; } = false;

        // Navigacijsko svojstvo - lista rezervacija za ovaj automobil
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();

        // Dostupna količina vozila (računa se na temelju potvrđenih rezervacija)
        [NotMapped]
        public int AvailableQuantity => Math.Max(Quantity - (Reservations?.Count(r => r.Status == ReservationStatus.Confirmed) ?? 0), 0);

        // Kategorija automobila (mali, srednji, veliki, luksuzni)
        [NotMapped]
        public string Category => DetermineCategory();

        // Cijena najma po danu, ovisno o kategoriji
        [NotMapped]
        public int RentalPricePerDay => DetermineRentalPrice();

        // Metoda za određivanje kategorije automobila
        private string DetermineCategory()
        {
            var smallCars = new[] { "Škoda Fabia", "VW Polo", "Audi A3", "Mercedes A klasa", "BMW 1" };
            var mediumCars = new[] { "Škoda Octavia", "Audi A4", "Mercedes C klasa", "VW Golf 8", "BMW 3" };
            var largeCars = new[] { "Škoda Superb", "Audi A6", "Mercedes E klasa", "Passat B8", "BMW 5" };
            var luxuryCars = new[] { "Mercedes S klasa", "Audi A8", "BMW 7", "VW Touareg", "Audi RS6" };

            string carFullName = $"{Brand} {Model}".ToLower();

            if (smallCars.Any(c => carFullName.Contains(c.ToLower()))) return "Mali";
            if (mediumCars.Any(c => carFullName.Contains(c.ToLower()))) return "Srednji";
            if (largeCars.Any(c => carFullName.Contains(c.ToLower()))) return "Veliki";
            if (luxuryCars.Any(c => carFullName.Contains(c.ToLower()))) return "Luksuzni";
            return "Nepoznato";
        }

        // Metoda za određivanje cijene najma po danu
        private int DetermineRentalPrice()
        {
            return Category switch
            {
                "Mali" => 50,
                "Srednji" => 150,
                "Veliki" => 300,
                "Luksuzni" => 450,
                _ => 0
            };
        }
    }
}
