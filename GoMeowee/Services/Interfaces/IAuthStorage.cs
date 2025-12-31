namespace GoMeowee.Services.Interfaces;

public interface IAuthStorage
{
    Task SaveTokenAsync(string token);
    Task<string?> GetTokenAsync();
    Task ClearAsync();
}
