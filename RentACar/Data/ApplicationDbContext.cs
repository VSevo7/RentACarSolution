using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentACar.Models;
using System;

namespace RentACar.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // ✅ Važno za Identity

            // ✅ Definiranje odnosa između automobila i rezervacija
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Car)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.CarId)
                .OnDelete(DeleteBehavior.Restrict); // Sprječava brisanje automobila ako ima aktivne rezervacije

            // ✅ Pozivamo metodu koja dodaje početne uloge i admin korisnika
            SeedRolesAndAdmin(modelBuilder);

            // ✅ Početni podaci za automobile (opcionalno)
            modelBuilder.Entity<Car>().HasData(
                new Car { Id = 1, Brand = "Škoda", Model = "Octavia", Transmission = "Automatski", Fuel = "Dizel", Quantity = 3 },
                new Car { Id = 2, Brand = "VW", Model = "Passat", Transmission = "Ručno", Fuel = "Benzin", Quantity = 2 },
                new Car { Id = 3, Brand = "Audi", Model = "A4", Transmission = "Automatski", Fuel = "Benzin", Quantity = 1 }
            );
        }

        // ✅ Dodavanje početnih uloga i administratora
        private void SeedRolesAndAdmin(ModelBuilder modelBuilder)
        {
            string adminRoleId = Guid.NewGuid().ToString();
            string userRoleId = Guid.NewGuid().ToString();
            string adminUserId = Guid.NewGuid().ToString();

            // ✅ Dodavanje uloga u bazu podataka
            var adminRole = new IdentityRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" };
            var userRole = new IdentityRole { Id = userRoleId, Name = "User", NormalizedName = "USER" };

            modelBuilder.Entity<IdentityRole>().HasData(adminRole, userRole);

            // ✅ Kreiranje admin korisnika
            var adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin@rentacar.com",
                NormalizedUserName = "ADMIN@RENTACAR.COM",
                Email = "admin@rentacar.com",
                NormalizedEmail = "ADMIN@RENTACAR.COM",
                EmailConfirmed = true,
                FullName = "Admin RentACar",
                SecurityStamp = Guid.NewGuid().ToString() // ✅ Generisan SecurityStamp
            };

            PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123!"); // ✅ Postavljena sigurna lozinka

            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            // ✅ Dodjela admin uloge admin korisniku
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = adminUserId
            });
        }
    }
}
