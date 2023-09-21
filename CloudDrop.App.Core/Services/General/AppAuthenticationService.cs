using CloudDrop.App.Core.Constants;
using CloudDrop.Shared.Models.Requests;
using CloudDrop.Shared.Models.Responses;

using Microsoft.Extensions.Logging;

using UzairAli.NetHttpClient;
using UzairAli.NetHttpClient.Exceptions;

namespace CloudDrop.App.Core.Services.General;
public class AppAuthenticationService
{
    private readonly ILogger<AppSessionService> _logger;
    private readonly IHttpClientService _httpClientService;

    public AppAuthenticationService(ILogger<AppSessionService> logger,
        IHttpClientService httpClientService)
    {
        _logger = logger;
        _httpClientService = httpClientService;
    }


    public async Task<AuthenticationResponse?> AuthenticateUserAsync(string apiUrl, string username, string password)
    {
        _logger.LogInformation($"Api: {apiUrl}");

        try
        {
            AuthenticationRequest req = new(username, password);

            HttpResponseMessage result = await _httpClientService.PostAsync(apiUrl + ApiConstants.SigninEndpoint, req);
            if (!result.IsSuccessStatusCode)
            {
                string content = await result.Content.ReadAsStringAsync();
                _logger.LogError(content);
                return default;
            }

            return await _httpClientService.DeserializeResponseAsync<AuthenticationResponse?>(result);
        }
        catch (InvalidJsonException ex)
        {
            _logger.LogError(ex.ContentString);
            _logger.LogInformation($"Api: {apiUrl}");
        }
        catch (Exception e)
        {
            _logger.LogInformation($"Api: {apiUrl}");
            _logger.LogError("Exception: {0} ", e.Message);
        }

        return default;
    }

}
