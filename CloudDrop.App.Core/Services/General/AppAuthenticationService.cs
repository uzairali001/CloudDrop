using CloudDrop.App.Core.Constants;
using CloudDrop.App.Core.Contracts.Services.Data;
using CloudDrop.App.Core.Models.Dtos;
using CloudDrop.App.Core.Models.Requests;
using CloudDrop.App.Core.Models.Responses;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using UzairAli.HttpClient;
using UzairAli.HttpClient.Exceptions;

namespace CloudDrop.App.Core.Services.General;
public class AppAuthenticationService
{
    public string? AuthToken { get; private set; }
    public string? BaseUrl { get; private set; }

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AppAuthenticationService> _logger;
    private readonly IAuthenticationService _authenticationService;

    public AppAuthenticationService(IServiceScopeFactory scopeFactory,
        ILogger<AppAuthenticationService> logger,
        IAuthenticationService authenticationService)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _authenticationService = authenticationService;
    }

    public async Task InitializeAsync()
    {
        AuthenticationDto dto = await _authenticationService.GetAsync();
        BaseUrl = await _authenticationService.GetBaseUrlAsync();

        if (AuthToken is null)
        {
            _logger.LogWarning("AuthToken is null");
        }

        if (BaseUrl is null)
        {
            _logger.LogWarning("BaseUrl is null");
        }
    }

    public async Task<SigninResponse?> AuthenticateUserAsync(string username, string password)
    {
        _logger.LogInformation($"Api: {BaseUrl}");

        try
        {
            if (string.IsNullOrEmpty(BaseUrl))
            {
                _logger.LogError("Baseurl is not set");
                return default;
            }

            var httpOptions = new UzairAli.HttpClient.Models.Configurations.HttpClientOptions()
            {
                BaseAddress = new Uri(BaseUrl)
            };
            var client = new HttpClientService(httpOptions);

            return await client.PostAsync<SigninResponse>(ApiConstants.SigninEndpoint, new SigninRequest()
            {
                Email = username,
                Password = password,
            });
        }
        catch (InvalidJsonException ex)
        {
            _logger.LogError(ex.ContentString);
            _logger.LogInformation($"Api: {BaseUrl}");

        }
        catch (HttpRequestException e)
        {
            _logger.LogInformation($"Api: {BaseUrl}");
            _logger.LogError("Exception: {0} ", e.Message);
        }

        return default;
    }

    public async Task SetBaseUrlAsync(string baseUrl)
    {
        baseUrl += baseUrl[^1] != '/' ? "/" : "";
        BaseUrl = baseUrl;
        await _authenticationService.AddOrUpdateBaseUrlAsync(baseUrl);
    }
    public async Task SetAuthenticationTokenAsync(string token)
    {
        AuthToken = token;
        await _authenticationService.AddOrUpdateAuthenticationTokenAsync(token);
    }
}
