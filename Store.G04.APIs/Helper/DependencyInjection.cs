using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using Store.G04.APIs.Errors;
using Store.G04.Core;
using Store.G04.Core.Dtos._Common;
using Store.G04.Core.Entities.Identity;
using Store.G04.Core.Mapping.Auth;
using Store.G04.Core.Mapping.Book;
using Store.G04.Core.Mapping.Machines;
using Store.G04.Core.Mapping.Orders;
using Store.G04.Core.Mapping.RawMaterials;
using Store.G04.Core.Mapping.Wishlist;
using Store.G04.Core.Repositories.Contract;
using Store.G04.Core.Services.Contract;
using Store.G04.Repository.Repositories;
using Store.G04.Repositpory;
using Store.G04.Repositpory.Data.Contexts;
using Store.G04.Repositpory.IDentity.Contexts;
using Store.G04.Repositpory.Repositories;
using Store.G04.Service.Services.Caches;
using Store.G04.Service.Services.EmailServices;
using Store.G04.Service.Services.Machine;
using Store.G04.Service.Services.Orders;
using Store.G04.Service.Services.Payments;
using Store.G04.Service.Services.RawMaterials;
using Store.G04.Service.Services.Users;
using Store.G04.Service.Tokens;
using System;
using System.Linq;
using System.Text;

namespace Store.G04.APIs.Helper
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddBuiltInServices()
                .AddSwaggerServices()
                .AddDbContextServices(configuration)
                .AddUserDefinedServices(configuration)
                .AddAutoMapperServices(configuration)
                .ConfigureInvalidModelStateResponse()
                .AddRedisServices(configuration)
                .AddIdentityServices()
                .AddAuthenticationServices(configuration);

            return services;
        }

        // Built-in services registration
        private static IServiceCollection AddBuiltInServices(this IServiceCollection services)
        {
            services.AddControllers();
            return services;
        }

        // Swagger services registration
        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }

        // Database context registration
        private static IServiceCollection AddDbContextServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<StoreIdentityDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));

            return services;
        }

        // Custom services registration
        private static IServiceCollection AddUserDefinedServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRawMaterialsService, RawMaterialService>();
            services.AddScoped<IMachinesService, MachineService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IWishlistRepository, WishlistRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddTransient(typeof(IEmailServices), typeof(EmailService));
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            return services;
        }

        // AutoMapper services registration
        private static IServiceCollection AddAutoMapperServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(M => M.AddProfile(new RawMaterialsProfile(configuration)));
            services.AddAutoMapper(M => M.AddProfile(new MachinesProfile(configuration)));
            services.AddAutoMapper(M => M.AddProfile(new WishlistProfile()));
            services.AddAutoMapper(M => M.AddProfile(new BookProfile()));
            services.AddAutoMapper(M => M.AddProfile(new AuthProfile()));
            services.AddAutoMapper(M => M.AddProfile(new OrderProfile(configuration)));
            return services;
        }

        // Invalid model state response configuration
        private static IServiceCollection ConfigureInvalidModelStateResponse(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(state => state.Value.Errors.Count > 0)
                        .SelectMany(state => state.Value.Errors)
                        .Select(error => error.ErrorMessage)
                        .ToArray();

                    var response = new ApiValidationErrorResponse { Errors = errors };
                    return new BadRequestObjectResult(response);
                };
            });

            return services;
        }

        // Redis services registration
        private static IServiceCollection AddRedisServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(_ =>
            {
                var connectionString = configuration.GetConnectionString("Redis");

                try
                {
                    var options = ConfigurationOptions.Parse(connectionString);
                    options.AbortOnConnectFail = false;
                    options.ConnectRetry = 3;
                    options.ConnectTimeout = 10000;

                    return ConnectionMultiplexer.Connect(options);
                }
                catch (RedisConnectionException ex)
                {
                    Console.WriteLine($"Failed to connect to Redis: {ex.Message}");
                    throw;
                }
            });

            return services;
        }

        // Identity services registration
        private static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();

            return services;
        }

        // Authentication and JWT services registration
        private static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["Jwt:Audience"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                });

            return services;
        }
    }
}