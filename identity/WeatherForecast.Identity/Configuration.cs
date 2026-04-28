using IdentityServer4;
using IdentityServer4.Models;
using IdentityModel;

namespace WeatherForecast.Identity;

public static class Configuration
{
    private static IConfiguration _configuration;

    public static void SetConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("WeatherForecastAPI", "Weather Forecast API")
        };
        
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };
        
    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new ApiResource("WeatherForecastAPI", "Weather Forecast API", new []
                { JwtClaimTypes.Name})
            {
                Scopes = {"WeatherForecastAPI"}
            }
        };
        
    public static IEnumerable<Client> Clients
    {
        get
        {
            var clientId = _configuration?["ClientSettings:ClientId"] ?? "weather-forecast-web-app";
            var clientName = _configuration?["ClientSettings:ClientName"] ?? "Weather Forecast Web";
            var redirectUris = _configuration?.GetSection("ClientSettings:RedirectUris").Get<string[]>()
                               ?? new[] { "http://localhost:3000/signin-oidc" };
            var allowedCorsOrigins = _configuration?.GetSection("ClientSettings:AllowedCorsOrigins").Get<string[]>()
                                     ?? new[] { "http://localhost:3000" };
            var postLogoutRedirectUris = _configuration?.GetSection("ClientSettings:PostLogoutRedirectUris").Get<string[]>()
                                         ?? new[] { "http://localhost:3000/signout-callback-oidc" };
            var accessTokenLifetime = _configuration?.GetValue<int>("ClientSettings:AccessTokenLifetime", 86400);

            return new List<Client>
            {
                new Client
                {
                    ClientId = clientId,
                    ClientName = clientName,
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    RedirectUris = redirectUris,
                    AllowedCorsOrigins = allowedCorsOrigins,
                    PostLogoutRedirectUris = postLogoutRedirectUris,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "WeatherForecastAPI"
                    },
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = accessTokenLifetime ?? 86400, // 24 часа для отладки
                    AllowOfflineAccess = true
                }
            };
        }
    }
}