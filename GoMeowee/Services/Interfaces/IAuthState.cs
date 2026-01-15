using GoMeowee.Models.Users;

namespace GoMeowee.Services.Interfaces;

public interface IAuthState
{
    bool IsAuthenticated { get; }
    public UserDetailsDto? CurrentUser { get; }
    event Action? AuthStateChanged;
    void SetAuthenticated(bool value, UserDetailsDto? userDetails = null);
}
