using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TouristAgency.Data;
using TouristAgency.Middleware;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        string connectionString = builder.Configuration.GetConnectionString("DbConnection");
        builder.Services.AddDbContext<TouristAgency1Context>(option => option.UseSqlServer(connectionString));
        builder.Services.AddDefaultIdentity<IdentityUser>().AddDefaultTokenProviders().AddRoles<IdentityRole>().AddEntityFrameworkStores<TouristAgency1Context>();
        builder.Services.AddControllersWithViews(options =>
        {
            options.CacheProfiles.Add("ModelCache",
                new CacheProfile()
                {
                    Location = ResponseCacheLocation.Any,
                    Duration = 2 * 11 + 240
                });
        });
        builder.Services.AddSession();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.UseStaticFiles();

        app.UseSession();

        app.UseDbInitializer();

        app.UseRouting();

        app.UseAuthorization();
        app.MapRazorPages();
        app.MapControllerRoute(
           name: "default",
           pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
