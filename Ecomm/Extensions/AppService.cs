using Ecomm.core.Interfaces;
using Ecomm.Errors;
using Ecomm.Helper;
using Ecomm.repository.Context;
using Ecomm.repository.Repository;
using Ecomm.service.ImplementServices;
using Ecomm.service.InterfaceServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
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
            //Add ratelimiting
            Services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("fixed", opt =>
                {
                    opt.Window = TimeSpan.FromMinutes(1);
                    opt.PermitLimit = 5;
                    opt.QueueLimit = 1;
                    opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                });
            });
            //dependencyInjection ICtegoryRepo
            Services.AddScoped(typeof(ICategoryRepo), typeof(CategoryRepo));
            //dependencyInjection IUnitOfWork
            Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            //dependencyInjection ICategoryService
            Services.AddScoped(typeof(ICategoryServices), typeof(CategoryServices));
            //automapper
            Services.AddAutoMapper(typeof(MappingProfile));
            //dependencyInjection IProductRepo
            Services.AddScoped(typeof(IProductRepo), typeof(ProductRepo));
            //dependencyInjection IProductService
            Services.AddScoped(typeof(IProductServices), typeof(ProductServices));
            //file provider
            Services.AddSingleton<IFileProvider>(
                 new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            //dependencyInjection IImagesetting
            Services.AddScoped(typeof(ImageSetting), typeof(ImageSetting));






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
                    message = "Resource not found, please check your endpoint and try again"
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