using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentACar.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ReservationId { get; set; }

        [ForeignKey("ReservationId")]
        public Reservation Reservation { get; set; } = null!; // ✅ Osiguravamo da nije null

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Iznos mora biti veći od 0!")]
        public decimal Amount { get; set; }

        [Required]
        public bool Paid { get; set; } = false; // ✅ Default: nije plaćeno

        [Required]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Broj kartice mora imati točno 16 znamenki!")]
        public string CardNumber { get; set; } = string.Empty; // ✅ Inicijalizirano

        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "CVC mora imati točno 3 znamenke!")]
        public string CVC { get; set; } = string.Empty; // ✅ Inicijalizirano

        [Required]
        [RegularExpression(@"^(0[1-9]|1[0-2])$", ErrorMessage = "Neispravan format mjeseca (MM)")]
        public string ExpiryMonth { get; set; } = string.Empty; // ✅ Inicijalizirano

        [Required]
        [RegularExpression(@"^\d{2}$", ErrorMessage = "Neispravan format godine (YY)")]
        public string ExpiryYear { get; set; } = string.Empty; // ✅ Inicijalizirano

        [Required]
        [StringLength(100, ErrorMessage = "Ime i prezime ne smije biti duže od 100 znakova!")]
        public string FullName { get; set; } = string.Empty; // ✅ Inicijalizirano

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.Now; // ✅ Automatsko postavljanje datuma plaćanja
    }
}
