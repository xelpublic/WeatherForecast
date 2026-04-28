using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Wf.Application.Common.Behaviors;
using Wf.Application.WeatherForecast.Queries.GetWeatherForecast;

namespace Wf.Application.Mappings;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.RegisterServicesFromAssembly(typeof(WeatherForecastVm).Assembly);
            }
        );

        services
            .AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });

        services.AddTransient(typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>),
            typeof(LoggingBehavior<,>));
        return services;
    }
}