using GoMeowee.Services.Interfaces;

namespace GoMeowee.Services;

public class AuthStorage : IAuthStorage
{
    private const string TokenKey = "auth_token";

    public async Task SaveTokenAsync(string token)
    {
        await SecureStorage.SetAsync(TokenKey, token);
    }

    public async Task<string?> GetTokenAsync()
    {
        try
        {
            return await SecureStorage.GetAsync(TokenKey);
        }
        catch
        {
            // device lock / biometric failure / OS weirdness
            return null;
        }
    }

    public Task ClearAsync()
    {
        SecureStorage.Remove(TokenKey);
        return Task.CompletedTask;
    }
}
