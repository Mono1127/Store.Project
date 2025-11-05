using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Store.Project.Domain.Contracts;
using Store.Project.Domain.Entities.Identity;
using Store.Project.Persistence;
using Store.Project.Persistence.Identity.Contexts;
using Store.Project.Services;
using Store.Project.Shared;
using Store.Project.Shared.ErrorModels;
using Store.Project.Web.Middlewares;
using System.Text;

namespace Store.Project.Web.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection RegisterAllService(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddIdentityServices();

            services.AddInfrastructureServices(configuration);
            services.AddApplicationService(configuration);
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

            var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";


            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey))

                };
            });

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
            await dbInitializer.InitializIdentityAsync();

            app.UseStaticFiles();
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();



            app.MapControllers();

            return app;

        }
        private static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentityCore<AppUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<IdentityStoreDbContext>();  
            return services;
        }

    }
}
