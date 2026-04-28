using Microsoft.AspNetCore.Identity;
using Serilog;
using WeatherForecast.Identity.Data;
using WeatherForecast.Identity.Models;

namespace WeatherForecast.Identity;

public class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("logs/identity-log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            Log.Information("Запуск Identity Service...");
            var host = CreateHostBuilder(args).Build();
                
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var context = serviceProvider.GetRequiredService<AuthDbContext>();
                    var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
                    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                        
                    await DbInitializer.InitializeAsync(context, userManager, roleManager);
                    Log.Information("База данных инициализирована");
                }
                catch (Exception exception)
                {
                    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(exception, "Произошла ошибка при инициализации приложения");
                }
            }
                
            await host.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Identity Service завершился с ошибкой");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}