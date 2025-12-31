using GoMeowee.Models;
using GoMeowee.Models.Auth;
using GoMeowee.Services.Interfaces;

namespace GoMeowee.Services;

public class AuthService : IAuthService
{
    private readonly ApiClient _apiClient;
    private readonly IAuthState _authState;
    private readonly IAuthStorage _authStorage;

    public AuthService(ApiClient apiClient, IAuthState authState, IAuthStorage authStorage)
    {
        _apiClient = apiClient;
        _authState = authState;
        _authStorage = authStorage;
    }

    public async Task<ApiResult<LoginResult>> LoginAsync(string username, string password)
    {
        var request = new LoginRequest
        {
            Username = username,
            Password = password
        };

        var result = await _apiClient.PostAsync<LoginResult>("api/Auth/login", request);

        if (result.IsSuccess)
        {
            var token = result.Response!.Token;

            _apiClient.SetToken(token);
            await _authStorage.SaveTokenAsync(token);

            var user = new UserDetailsDto()
            {
                Id          = result.Response.UserId,
                Username    = result.Response.Username,
                DisplayName = result.Response.DisplayName,
                AvatarUrl   = result.Response.AvatarUrl
            };

            _authState.SetAuthenticated(true, user);
        }
        
        return result;
    }

    public async Task LogoutAsync()
    {
        _apiClient.SetToken(null);
        _authState.SetAuthenticated(false);
        await _authStorage.ClearAsync();
    }

    public async Task RestoreAuthStateAsync()
    {
        var token = await _authStorage.GetTokenAsync();

        if (!string.IsNullOrWhiteSpace(token))
        {
            _apiClient.SetToken(token);
            var result = await _apiClient.GetAsync<UserDetailsDto>($"api/Users/me");

            if (result.IsSuccess && result.Response is not null)
            {
                _authState.SetAuthenticated(true, result.Response);
            }
            else
                _authState.SetAuthenticated(false);
        }
    }
}
