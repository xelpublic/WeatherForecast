using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Wf.Application.Common.Mappings;
using Wf.Application.Interfaces;
using Wf.Application.Mappings;
using Wf.Application.WeatherForecast.Queries.GetWeatherForecast;
using Wf.Infrastructure.WeatherApiCom;
using Wf.Infrastructure.WeatherForecast;
using Wf.Infrastructure.WeatherPersistanceCache;

namespace Wf.WebApi;

public class Startup
{
    private IConfiguration _сonfiguration;

    public Startup(IConfiguration configuration) => _сonfiguration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
            config.AddProfile(new AssemblyMappingProfile(typeof(WeatherForecastVm).Assembly));
        });

        services.AddApplication();
        services.AddControllers();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            });
        });

        services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen();
        services.AddApiVersioning();
        
        services.AddHttpContextAccessor();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = _сonfiguration.GetConnectionString("Redis");
            options.InstanceName = "WeatherForecast_";
        });
        services.AddScoped<ICacheService, RedisCacheService>();

        services.AddTransient<IWeatherApiComService, WeatherApiComService>();
        services.AddTransient<IWeatherForecastService, WeatherForecastService>();
        services.AddHttpClient();
        services.AddHealthChecks();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
        IApiVersionDescriptionProvider provider)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI(config =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                config.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
                config.RoutePrefix = string.Empty;
            }
        });
        app.UseRouting();
        app.UseCors("AllowAll");
        app.UseApiVersioning();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health");
        });
    }
}