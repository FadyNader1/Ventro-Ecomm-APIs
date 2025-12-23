using Ecomm.core.Entities.EmailSetting;
using Ecomm.core.Entities.IdentityModle;
using Ecomm.core.Interfaces;
using Ecomm.Errors;
using Ecomm.Helper;
using Ecomm.repository.Context;
using Ecomm.repository.Repository;
using Ecomm.service.ImplementServices;
using Ecomm.service.InterfaceServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuestPDF.Infrastructure;
using StackExchange.Redis;
using System.Text;
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

            // أضف رابط الـ Vercel الخاص بك هنا
            Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.WithOrigins(
                            "http://localhost:4200",
                            "https://localhost:4200",
                            "https://ventro-epwz.vercel.app" // الرابط الجديد من الصورة
                          )
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });
            //Add ratelimiting
            Services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("fixed", opt =>
                {
                    opt.Window = TimeSpan.FromMinutes(1);
                    opt.PermitLimit = 100;
                    opt.QueueLimit = 1;
                    opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                });
            });
            //Redispatching
            Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var connect = configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connect!);
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
            //dependencyInjection ICustomerBasket
            Services.AddScoped(typeof(ICustomerBasket), typeof(CustomerBasketService));
            //DI IAth
            Services.AddScoped(typeof(IAuth), typeof(Auth));
            //HttpContextAccessores
            Services.AddHttpContextAccessor();
            //emailsetting
            Services.Configure<EmailConfig>(configuration.GetSection("Email"));
            //DI IEmailSetting
            Services.AddScoped(typeof(IEmailSetting), typeof(EmailSetting));
            //DI ITokenSetting
            Services.AddScoped(typeof(ITokenSetting), typeof(TokenSetting));
            //DI IRefreshTokenRepo
            Services.AddScoped(typeof(IRefreshTokenRepo), typeof(RefreshTokenRepo));
            //DI OrderRepo
            Services.AddScoped(typeof(IOrderRepo), typeof(OrderRepo));
            //DI OrderService
            Services.AddScoped(typeof(IOrderService), typeof(OrderService));
            //DI DeliveryMethodRepo
            Services.AddScoped(typeof(IDeliveryMethodRepo), typeof(DeliveryMethodRepo));
            //DI DeliveryMethodService
            Services.AddScoped(typeof(IDeliveryMethodService), typeof(DeliveryMethodService));
            //DI PaymentInent
            Services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            //DI WishListRepo
            Services.AddScoped(typeof(IWishListRepo), typeof(WishListRepo));
            //DI WishListService
            Services.AddScoped(typeof(IWishListService), typeof(WishListService));
            //QuestPDF license
            QuestPDF.Settings.License = LicenseType.Community;
            //DI IPdfGenerator
            Services.AddScoped<IPdfGenerator, PdfGenerator>();

            //Identity
            Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;


            }).AddEntityFrameworkStores<DBContext>()
            .AddDefaultTokenProviders();

            Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
                {
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["Token"];
                            return Task.CompletedTask;
                        }
                    };
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false,
                        ValidAudience = configuration["Jwt:Audience"],
                        ValidateIssuer = false,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!))
                    };
                }).AddGoogle(options =>
                {
                    options.ClientId = configuration["Authentication:Google:ClientId"]!;
                    options.ClientSecret = configuration["Authentication:Google:ClientSecret"]!;
                }).AddFacebook(options =>
                {
                    options.AppId = configuration["Authentication:Facebook:AppId"]!;
                    options.AppSecret = configuration["Authentication:Facebook:AppSecret"]!;
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
               // await ProductContextSeeding.SeedProducts(context);
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