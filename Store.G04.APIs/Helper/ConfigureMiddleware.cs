using Store.G04.APIs.Middlewares;
using Store.G04.Repositpory.Data.Contexts;
using Store.G04.Repositpory.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Store.G04.Repositpory.IDentity.Contexts;
using Store.G04.Repositpory.IDentity;
using Store.G04.Core.Entities.Identity;

namespace Store.G04.APIs.Helper;
public static class ConfigureMiddleware
{
    public static async Task<WebApplication> ConfigureMiddlewareAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<StoreDbContext>();
        var identityContext = services.GetRequiredService<StoreIdentityDbContext>();
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();

        try
        {
            await context.Database.MigrateAsync();
            await StoreDbContextSeed.SeedAsync(context);
            await identityContext.Database.MigrateAsync();
            await StoreIdentityDbContextSeed.SeedAppUserAsync(userManager);
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogError(ex, "There are problems during applying migrations!");
        }

        // ✅ Correct middleware order
        app.UseMiddleware<ExceptionMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseStatusCodePagesWithReExecute("/error/{0}");
        app.UseStaticFiles();
        app.UseHttpsRedirection();

        // ✅ Add this
        app.UseRouting();

        // ✅ Move CORS after UseRouting, before Authentication/Authorization
        app.UseCors("AllowAngularApp");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
