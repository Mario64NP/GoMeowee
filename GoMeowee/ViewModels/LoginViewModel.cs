using GoMeowee.Services.Interfaces;
using System.Windows.Input;

namespace GoMeowee.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly IAuthService _authService;

    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string? ErrorMessage { get; private set; }
    public bool HasError => ErrorMessage is not null;

    public ICommand LoginCommand { get; }

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;

        LoginCommand = new Command(async () => await LoginAsync());
    }


    public async Task LoginAsync()
    {
        try
        {
            IsBusy = true;
            ErrorMessage = null;
            OnPropertyChanged(nameof(ErrorMessage));

            var result = await _authService.LoginAsync(Username, Password);

            if (!result.IsSuccess)
            {
                ErrorMessage = result.ApiError;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            OnPropertyChanged(nameof(ErrorMessage));
        }
        finally
        {
            IsBusy = false;
        }
    }
}
