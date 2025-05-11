using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Identity;

namespace AgriEnergyConnect.Data
{
    public static class DbSeeder
    {
        public static void Seed(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            context.Database.EnsureCreated();

            // 1. Create Roles
            string[] roles = { "Farmer", "Employee" };
            foreach (var role in roles)
            {
                if (!roleManager.RoleExistsAsync(role).Result)
                {
                    roleManager.CreateAsync(new IdentityRole(role)).Wait();
                }
            }

            // 2. Create Admin User
            if (userManager.FindByEmailAsync("admin@agrienergy.com").Result == null)
            {
                var adminUser = new IdentityUser
                {
                    UserName = "admin@agrienergy.com",
                    Email = "admin@agrienergy.com",
                    EmailConfirmed = true
                };

                var result = userManager.CreateAsync(adminUser, "Admin@123").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(adminUser, "Employee").Wait();
                }
            }

            // 3. Seed Farmers
            if (!context.Farmers.Any())
            {
                var farmer1 = new Farmer { Name = "John Dlamini", Email = "john@example.com", ContactNumber = "0123456789", FarmLocation = "Limpopo" };
                var farmer2 = new Farmer { Name = "Lebo Khumalo", Email = "lebo@example.com", ContactNumber = "0987654321", FarmLocation = "Mpumalanga" };

                context.Farmers.AddRange(farmer1, farmer2);
                context.SaveChanges();

                // Seed Products
                context.Products.AddRange(
                    new Product { Name = "Organic Tomatoes", Category = "Vegetables", ProductionDate = DateTime.Now.AddDays(-10), FarmerId = farmer1.FarmerId },
                    new Product { Name = "Green Beans", Category = "Vegetables", ProductionDate = DateTime.Now.AddDays(-5), FarmerId = farmer2.FarmerId }
                );
                context.SaveChanges();
            }

            // 4. Seed Employees
            if (!context.Employees.Any())
            {
                context.Employees.Add(new Employee
                {
                    Name = "Sizwe Mokoena",
                    Email = "sizwe@connect.co.za",
                    Role = "Operations Officer"
                });

                context.SaveChanges();
            }
        }
    }
}
