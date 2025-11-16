using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.Project.Services.Abstractions;
using Store.Project.Services.Mapping.Basket;
using Store.Project.Services.Mapping.Orders;
using Store.Project.Services.Mapping.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Project.Services
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection service, IConfiguration configuration) 
        {
            service.AddAutoMapper(M => M.AddProfile(new ProductProfile(configuration)));
            service.AddAutoMapper(M => M.AddProfile(new BasketProfile()));
            service.AddAutoMapper(M => M.AddProfile(new OrderProfile()));
            service.AddScoped<IServiceManager, ServiceManager>();

            return service ;
        }
    }
}
