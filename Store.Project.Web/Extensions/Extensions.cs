using Microsoft.AspNetCore.Mvc;
using Store.Project.Domain.Contracts;
using Store.Project.Persistence;
using Store.Project.Services;
using Store.Project.Shared.ErrorModels;
using Store.Project.Web.Middlewares;

namespace Store.Project.Web.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection RegisterAllService(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddInfrastructureServices(configuration);
            services.AddApplicationService(configuration);



            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(m => m.Value.Errors.Any())
                     .Select(m => new ValidationError()
                     {
                         Field = m.Key,
                         Errors = m.Value.Errors.Select(errors => errors.ErrorMessage)

                     });

                    var response = new ValidationErrorResponse()
                    {
                        Errors = errors

                    };
                    return new BadRequestObjectResult(response);
                };
            });
            return services;
        }


        public static async Task<WebApplication> ConfigureMiddlewares(this WebApplication app) 
        {
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializAsync();

            app.UseStaticFiles();
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            return app;

        }



    }
}
