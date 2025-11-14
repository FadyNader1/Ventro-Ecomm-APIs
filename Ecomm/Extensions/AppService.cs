using Ecomm.Errors;
using Ecomm.repository.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ecomm.Extensions
{
    public static class AppService
    {



        public static IServiceCollection AddAppService(this IServiceCollection Services, IConfiguration configuration)
        {
            // Add services to the container.
            Services.AddControllers();
            //apivalidationEnhancement
            Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value?.Errors.Count > 0)
                        .SelectMany(x => x.Value!.Errors)
                        .Select(x => x.ErrorMessage).ToArray();
                    var toReturn = new ApiValidationError()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(toReturn);
                };
            });
            //addDbContext
            Services.AddDbContext<DBContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            //Add CORS policy
            Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });

            return Services;
        }

        public static IServiceCollection AddSwaggerService(this IServiceCollection Services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen();

            return Services;
        }

        public static async Task<WebApplication> UseAppService(this WebApplication app)
        {

            //notfoundEndpointEnhancement
            app.MapFallback(async context =>
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = "Resource not found"
                });
            });

            //migrate database during startup
            var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var context = services.GetRequiredService<DBContext>();
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                logger.CreateLogger<Program>().LogError($"Error occured during migration, {ex}");
            }


            return app;
        }

        public static WebApplication UseSwaggerService(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;

        }
    }
}