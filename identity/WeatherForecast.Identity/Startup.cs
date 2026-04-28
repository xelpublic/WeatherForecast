using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Microsoft.EntityFrameworkCore;
using WeatherForecast.Identity.Data;
using WeatherForecast.Identity.Models;

namespace WeatherForecast.Identity;

public class Startup
{
    public IConfiguration AppConfiguration { get; }

    public Startup(IConfiguration configuration) =>
        AppConfiguration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        Configuration.SetConfiguration(AppConfiguration);
            
        var connectionString = AppConfiguration.GetConnectionString("DbConnection");
            
        services.AddDbContext<AuthDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });
            
        services.AddIdentity<AppUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = AppConfiguration.GetValue<int>("ApplicationSettings:Password:RequiredLength", 3);
                config.Password.RequireDigit = AppConfiguration.GetValue<bool>("ApplicationSettings:Password:RequireDigit", false);
                config.Password.RequireNonAlphanumeric = AppConfiguration.GetValue<bool>("ApplicationSettings:Password:RequireNonAlphanumeric", false);
                config.Password.RequireUppercase = AppConfiguration.GetValue<bool>("ApplicationSettings:Password:RequireUppercase", false);
                config.Password.RequireLowercase = AppConfiguration.GetValue<bool>("ApplicationSettings:Password:RequireLowercase", false);
            })
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();
            
        services.AddIdentityServer(options =>
            {
                var issuerUri = AppConfiguration["IdentityServer:IssuerUri"];
                if (!string.IsNullOrEmpty(issuerUri))
                {
                    options.IssuerUri = issuerUri;
                }
            })
            .AddAspNetIdentity<AppUser>()
            .AddInMemoryApiResources(Configuration.ApiResources)
            .AddInMemoryIdentityResources(Configuration.IdentityResources)
            .AddInMemoryApiScopes(Configuration.ApiScopes)
            .AddInMemoryClients(Configuration.Clients)
            .AddDeveloperSigningCredential();
            
        services.ConfigureApplicationCookie(config =>
        {
            config.Cookie.Name = AppConfiguration["ApplicationSettings:Cookie:Name"];
            config.LoginPath = AppConfiguration["ApplicationSettings:Cookie:LoginPath"];
            config.LogoutPath = AppConfiguration["ApplicationSettings:Cookie:LogoutPath"];
            config.AccessDeniedPath = AppConfiguration["ApplicationSettings:Cookie:AccessDeniedPath"];
            // Для отладки по HTTP
            config.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
            config.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.None;
        });
            
        services.ConfigureExternalCookie(config =>
        {
            config.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
            config.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.None;
        });
            
        services.AddControllersWithViews();
        services.AddHealthChecks();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
            
        app.UseStaticFiles();
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
                Path.Combine(env.ContentRootPath, "Styles")),
            RequestPath = "/styles"
        });

        app.UseRouting();
            
        app.UseIdentityServer();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            endpoints.MapHealthChecks("/health");
        });
    }
}