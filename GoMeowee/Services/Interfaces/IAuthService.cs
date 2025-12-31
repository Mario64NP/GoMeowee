using GoMeowee.Models;
using GoMeowee.Models.Auth;

namespace GoMeowee.Services.Interfaces;

public interface IAuthService
{
    Task<ApiResult<LoginResult>> LoginAsync(string username, string password);
    Task LogoutAsync();
    Task RestoreAuthStateAsync();
}
