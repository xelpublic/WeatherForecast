using Serilog;
using Serilog.Events;
using Wf.Frontend.Helpers;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.File("logs/WfFrontendLog-.txt", rollingInterval:
        RollingInterval.Day)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    
    builder.Host.UseSerilog();

    builder.Services.AddRazorPages();
    builder.Services.AddHttpClient();
    builder.Services.AddHealthChecks();
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddTransient<AuthTokenHandler>();

    var identityAuthority = builder.Configuration["Identity:Authority"] ?? "http://localhost:5002";
    var metadataAddress = builder.Configuration["Identity:MetadataAddress"];
    var clientId = builder.Configuration["Identity:ClientId"] ?? "weather-forecast-web-app";

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies", options =>
    {
        options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
        options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.None;
    })
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = identityAuthority;
        options.ClientId = clientId;
        if (!string.IsNullOrEmpty(metadataAddress))
        {
            options.MetadataAddress = metadataAddress;
        }
        options.ResponseType = "code";
        options.UsePkce = true;
        options.ResponseMode = "query";
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("WeatherForecastAPI");
        options.Scope.Add("offline_access");
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        
        // Отключение требования HTTPS для отладки
        options.RequireHttpsMetadata = false;
        
        options.Events = new Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectEvents
        {
            OnRedirectToIdentityProvider = context =>
            {
                if (context.ProtocolMessage.IssuerAddress != null)
                {
                    var issuerAddress = context.ProtocolMessage.IssuerAddress;
                    Console.WriteLine($"Original issuer address: {issuerAddress}");
                    issuerAddress = issuerAddress.Replace("http://identity", "http://localhost:8081");
                    issuerAddress = issuerAddress.Replace("http://host.docker.internal:8081", "http://localhost:8081");
                    context.ProtocolMessage.IssuerAddress = issuerAddress;
                    Console.WriteLine($"Modified issuer address: {issuerAddress}");
                }
                return Task.CompletedTask;
            }
        };
    });

    var backendApiUrl = builder.Configuration["BackendApi:BaseUrl"] ?? "http://localhost:5000";
    var timeoutSeconds = builder.Configuration.GetValue<int>("BackendApi:TimeoutSeconds", 30);

    // Настройка HTTP-клиента для webapi
    builder.Services.AddHttpClient("WeatherApi", client =>
    {
        client.BaseAddress = new Uri(backendApiUrl);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
    })
    .AddHttpMessageHandler<AuthTokenHandler>();

    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }
    
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapRazorPages();
    app.MapHealthChecks("/health");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Wf.Frontend навернулся с ошибкой");
}
finally
{
    Log.CloseAndFlush();
}