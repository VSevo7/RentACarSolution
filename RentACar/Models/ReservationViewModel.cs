using System.Collections.Generic;

namespace RentACar.Models
{
    public class ReservationViewModel
    {
        public List<Reservation> UserReservations { get; set; } = new List<Reservation>(); // ✅ Korisnik vidi samo svoje rezervacije
        public List<Reservation> AllReservations { get; set; } = new List<Reservation>(); // ✅ Admin vidi sve rezervacije
        public List<Car> AvailableCars { get; set; } = new List<Car>(); // ✅ Dostupni automobili

        // ✅ Konstruktor da osiguramo da liste nisu null
        public ReservationViewModel()
        {
            UserReservations = new List<Reservation>();
            AllReservations = new List<Reservation>();
            AvailableCars = new List<Car>();
        }
    }
}
