using Microsoft.EntityFrameworkCore;
using Store.Project.Domain.Contracts;
using Store.Project.Domain.Entities.Products;
using Store.Project.Persistence.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Project.Persistence
{
    public class DbInitializer(StoreDbContext _context) : IDbInitializer
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
    }
}
