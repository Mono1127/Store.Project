using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.Project.Services.Abstractions;
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
            service.AddScoped<IServiceManager, ServiceManager>();

            return service ;
        }
    }
}
