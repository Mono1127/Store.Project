using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.Project.Domain.Contracts;
using Store.Project.Domain.Entities.Identity;
using Store.Project.Domain.Entities.Products;
using Store.Project.Persistence.Data.Contexts;
using Store.Project.Persistence.Identity.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Project.Persistence
{
    public class DbInitializer(
        StoreDbContext _context
        ,IdentityStoreDbContext identity
        ,UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager) : IDbInitializer
    {
       
        public async Task InitializAsync()
        {
            if ( _context.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Any() )
            {
                await _context.Database.MigrateAsync();
            }
            if (!_context.ProductBrands.Any()) 
            {
                //\Infrastructure\Store.Project.Presistence\Data\DataSeeding\brands.json
                var brandsdata = await File.ReadAllTextAsync(@"..\Infrastructure\Store.Project.Presistence\Data\DataSeeding\brands.json");

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsdata);

                if (brands is not null && brands.Count > 0)
                {
                    await _context.ProductBrands.AddRangeAsync(brands);

                }

            }
            if (!_context.ProductType.Any())
            {
                //\Infrastructure\Store.Project.Presistence\Data\DataSeeding\types.json
                var typesdata = await File.ReadAllTextAsync(@"..\Infrastructure\Store.Project.Presistence\Data\DataSeeding\types.json");

                var types = JsonSerializer.Deserialize<List<ProductType>>(typesdata);

                if (types is not null && types.Count > 0)
                {
                    await _context.ProductType.AddRangeAsync(types);

                }


            }
            if (!_context.Products.Any())
            {
                //\Infrastructure\Store.Project.Presistence\Data\DataSeeding\products.json
                var productsdata = await File.ReadAllTextAsync(@"..\Infrastructure\Store.Project.Presistence\Data\DataSeeding\Products.json");

                var products = JsonSerializer.Deserialize<List<Product>>(productsdata);

                if (products is not null && products.Count > 0)
                {
                    await _context.Products.AddRangeAsync(products);

                }


            }


           await _context.SaveChangesAsync(); 

        }

        public async Task InitializIdentityAsync()
        {
            if (identity.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Any())
            {
                await identity.Database.MigrateAsync();
            }


            //Data Seed
            if (!identity.Roles.Any()) 
            {
              await  roleManager.CreateAsync(new IdentityRole() { Name = "SuperAdmin" });
              await  roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });

            }

            if (!identity.Users.Any())
            {
                var superAdmin = new AppUser()
                {
                    UserName = "SuperAdmin",
                    DisplayName = "SuperAdmin",
                    Email = "SuperAdmin@gmail.com",
                    PhoneNumber = "1234567890"
                };

                var Admin = new AppUser()
                {
                    UserName = "Admin",
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    PhoneNumber = "12345672134"
                };

                await userManager.CreateAsync(Admin, "P@ssW0rd");
                await userManager.CreateAsync(superAdmin, "P@ssW0rd");
                await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                await userManager.AddToRoleAsync(Admin, "Admin");

              


            }


        }
    }
}
