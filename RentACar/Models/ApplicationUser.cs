using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RentACar.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        // Dodatna polja ako želiš proširiti korisnički model
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
    }
}
