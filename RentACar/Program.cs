using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RentACar.Data;
using RentACar.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Dodavanje servisa u DI (Dependency Injection)
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // ✅ Potrebno za Login i Register

// Registracija ApplicationDbContext-a s konekcijom na bazu
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dodavanje Identity servisa
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // ✅ Ako želiš potvrdu emaila, postavi na true
    options.Password.RequireDigit = true; // ✅ Zahtjeva broj u lozinci
    options.Password.RequireLowercase = true; // ✅ Zahtjeva mala slova
    options.Password.RequireUppercase = true; // ✅ Zahtjeva velika slova
    options.Password.RequiredLength = 6; // ✅ Minimalna duljina lozinke
})
.AddRoles<IdentityRole>() // ✅ Dodana podrška za uloge (Admin, User)
.AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

// ✅ Automatski kreiraj uloge i admin korisnika ako ne postoje
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedRolesAndAdmin(services);
}

// Middleware konfiguracija
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); // ✅ Omogućava prijavu korisnika
app.UseAuthorization();

// Rutiranje
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ✅ Omogućavanje Identity Razor Pages (Login, Register)
app.MapRazorPages();

app.Run();

// ✅ Funkcija za automatsko kreiranje uloga i admin korisnika
async Task SeedRolesAndAdmin(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roles = { "Admin", "User" };

    // Kreiranje uloga ako ne postoje
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Kreiranje admin korisnika ako ne postoji
    var adminEmail = "admin@rentacar.com";

    // 🛠 FIX: Umjesto FindByEmailAsync, koristimo FirstOrDefaultAsync() kako bismo izbjegli grešku s duplikatima
    var existingAdmins = await userManager.Users
        .Where(u => u.Email == adminEmail)
        .ToListAsync();

    // Ako postoji više admina s istim emailom, obriši duplikate
    if (existingAdmins.Count > 1)
    {
        var adminToKeep = existingAdmins.First();
        var adminsToDelete = existingAdmins.Skip(1); // Svi osim prvog

        foreach (var admin in adminsToDelete)
        {
            await userManager.DeleteAsync(admin);
        }
    }

    // Ponovno dohvaćanje admina nakon brisanja duplikata
    var existingAdmin = await userManager.Users
        .Where(u => u.Email == adminEmail)
        .FirstOrDefaultAsync();

    if (existingAdmin == null)
    {
        var adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            FullName = "Admin RentACar"
        };

        var result = await userManager.CreateAsync(adminUser, "Admin123!");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}
