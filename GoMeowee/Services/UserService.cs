using GoMeowee.Models.Interests;
using GoMeowee.Models.Users;

namespace GoMeowee.Services;

public class UserService(ApiClient apiClient)
{
    public async Task<UserDetailsDto?> GetCurrentUserAsync()
    {
        var result = await apiClient.GetAsync<UserDetailsDto>("api/Users/me");

        if (!result.IsSuccess)
            return null;

        return result.Response;
    }

    public async Task<UserDetailsDto?> GetUserByUsernameAsync(string username)
    {
        var result = await apiClient.GetAsync<UserDetailsDto>($"api/Users/{username}");

        if (!result.IsSuccess)
            return null;

        return result.Response;
    }

    public async Task<IEnumerable<UserInterestDto>?> GetInterestedEventsByUserAsync(string username)
    {
        var result = await apiClient.GetAsync<IEnumerable<UserInterestDto>>($"api/Users/{username}/interests");

        if (!result.IsSuccess)
            return null;

        return result.Response;
    }

    public async Task<bool> UploadAvatar()
    {
        return true; 
    }
}
