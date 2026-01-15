using GoMeowee.Services.Interfaces;
using System.Windows.Input;

namespace GoMeowee.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    private readonly IAuthService _authService;

    public ICommand GoBackCommand { get; }
    public ICommand LogoutCommand { get; }
    public ICommand OpenGitHubIssuesCommand { get; }

    public string AppVersion => "1.0.0";

    public SettingsViewModel(IAuthService authService)
    {
        _authService = authService;

        GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        LogoutCommand = new Command(async () => await LogoutAsync());
        OpenGitHubIssuesCommand = new Command(async () => await Launcher.Default.OpenAsync("https://github.com/Mario64NP/GoMeowee/issues"));
    }

    private async Task LogoutAsync()
    {
        bool confirm = await Shell.Current.DisplayAlertAsync(
            "Log out",
            "Are you sure you want to log out?",
            "Log out",
            "Cancel");

        if (confirm)
        {
            await _authService.LogoutAsync();
        }
    }
}
