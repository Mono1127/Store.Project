
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Project.Domain.Contracts;
using Store.Project.Persistence;
using Store.Project.Persistence.Data.Contexts;
using Store.Project.Services;
using Store.Project.Services.Abstractions;
using Store.Project.Services.Mapping.Products;
using Store.Project.Shared.ErrorModels;
using Store.Project.Web.Extensions;
using Store.Project.Web.Middlewares;
using System.Threading.Tasks;

namespace Store.Project.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

           builder.Services.RegisterAllService(builder.Configuration);

            var app = builder.Build();

            await app.ConfigureMiddlewares();

            app.Run();
            //
        }
    }
}
