using GoMeowee.Models;
using GoMeowee.Services.Interfaces;

namespace GoMeowee.Services;

public class AuthState : IAuthState
{
    public bool IsAuthenticated { get; private set; }
    public UserDetailsDto? CurrentUser { get; private set; }

    public event Action? AuthStateChanged;

    public void SetAuthenticated(bool value, UserDetailsDto? userDetails = null)
    {
        if (IsAuthenticated == value || (value && userDetails is null))
            return;

        IsAuthenticated = value;
        CurrentUser = userDetails;
        AuthStateChanged?.Invoke();
    }
}
