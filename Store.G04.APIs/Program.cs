using Store.G04.APIs.Helper;

namespace Store.G04.APIs;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAngularApp", policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });
        builder.Services.AddDependency(builder.Configuration);
        var app = builder.Build();
        await app.ConfigureMiddlewareAsync();
        app.Run();
    }
}
