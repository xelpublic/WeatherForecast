using System.Net.Http.Headers;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Wf.Frontend.Helpers;

/// <summary>
/// добавляет токен аутентификации в заголовок Authorization для запросов API.
/// </summary>
public class AuthTokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthTokenHandler> _logger;

    public AuthTokenHandler(IHttpContextAccessor httpContextAccessor, ILogger<AuthTokenHandler> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("AuthTokenHandler: SendAsync вызван для {RequestUri}", request.RequestUri);
        
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            var accessToken = await httpContext.GetTokenAsync("access_token");
            
            if (!string.IsNullOrEmpty(accessToken))
            {
                _logger.LogInformation("AuthTokenHandler: Найден access token, добавляем заголовок Authorization.");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
            else
            {
                _logger.LogWarning("AuthTokenHandler: Access token равен null или пуст. Попытка получить из альтернативных источников.");
                var authenticateResult = await httpContext.AuthenticateAsync();
                if (authenticateResult.Succeeded)
                {
                    var tokens = authenticateResult.Properties.GetTokens();
                    if (tokens.Any())
                    {
                        _logger.LogInformation("AuthTokenHandler: Доступные имена токенов: {TokenNames}",
                            string.Join(", ", tokens.Select(t => t.Name)));
                        var altToken = tokens.FirstOrDefault(t => t.Name == "access_token");
                        if (altToken != null)
                        {
                            accessToken = altToken.Value;
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                            _logger.LogInformation("AuthTokenHandler: Добавлен access token из альтернативного источника.");
                        }
                    }
                    else
                    {
                        _logger.LogWarning("AuthTokenHandler: Токены не найдены в свойствах аутентификации.");
                    }
                }
                else
                {
                    _logger.LogWarning("AuthTokenHandler: Пользователь не аутентифицирован.");
                }
            }
        }
        else
        {
            _logger.LogWarning("AuthTokenHandler: HttpContext равен null.");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}