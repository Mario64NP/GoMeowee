using GoMeowee.Models;

namespace GoMeowee.Services;

public class UserService(ApiClient client)
{
    private readonly ApiClient _apiClient = client;

    public async Task<UserDetailsDto?> GetMyProfileAsync()
    {
        var result = await _apiClient.GetAsync<UserDetailsDto>($"api/Users/me");

        if (!result.IsSuccess)
            return null;

        return result.Response;
    }

    public async Task<UserDetailsDto?> GetUserByUsernameAsync(string username)
    {
        var result = await _apiClient.GetAsync<UserDetailsDto>($"api/Users/{username}");

        if (!result.IsSuccess)
            return null;

        return result.Response;
    }

    public async Task<bool> UploadAvatar()
    {
        return true; 
    }
}
