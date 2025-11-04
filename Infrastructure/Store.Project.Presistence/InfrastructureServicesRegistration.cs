using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.Project.Domain.Contracts;
using Store.Project.Persistence.Data.Contexts;
using Store.Project.Services;
using Store.Project.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Persistence
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
            );

           services.AddScoped<IDbInitializer, DbInitializer>();
           services.AddScoped<IUnitOfWork, UnitOfWork>();
           

            return services;
        }

    }
}
