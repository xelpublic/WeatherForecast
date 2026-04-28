using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Wf.WebApi;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            var apiVersion = description.ApiVersion.ToString();
            options.SwaggerDoc(description.GroupName,
                new OpenApiInfo
                {
                    Version = apiVersion,
                    Title = $"Weather Forecast API {apiVersion}",
                    Description = "песочно‑демонстрационное ASP.NET Web API",
                    TermsOfService = new Uri("https://github.com/xelpublic"),
                    Contact = new OpenApiContact
                    {
                        Name = "xelandr",
                        Email = string.Empty,
                        Url = new Uri("https://t.me/xelandr")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "xelandr",
                        Url = new Uri("https://t.me/xelandr")
                    }
                });

            options.CustomOperationIds(apiDescription =>
                apiDescription.TryGetMethodInfo(out MethodInfo methodInfo)
                    ? methodInfo.Name
                    : null);
        }
    }
}