using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentACar.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public int CarId { get; set; }

        [ForeignKey("CarId")]
        public Car Car { get; set; } = null!; // Inicijalizirano da spriječi CS8618

        [Required]
        public string UserId { get; set; } = string.Empty; // Osigurano da nije null

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!; // Inicijalizirano da spriječi CS8618

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending; // Defaultno status je "Pending"

        [NotMapped]
        public bool IsPaid { get; set; } = false; // Defaultna vrijednost

        // ✅ Konstruktor koji osigurava da navigacijska svojstva nisu null
        public Reservation()
        {
            Car = new Car();
            User = new ApplicationUser();
        }
    }

    public enum ReservationStatus
    {
        Pending,    // Čeka odobrenje admina
        Confirmed,  // Prihvaćeno
        Rejected,   // Odbijeno
        Canceled    // Korisnik je otkazao
    }
}
