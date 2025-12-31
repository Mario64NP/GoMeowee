using GoMeowee.Services.Interfaces;

namespace GoMeowee.Views;

public partial class SettingsPage : ContentPage
{
	private readonly IAuthService _authService;
	public SettingsPage(IAuthService authService)
	{
		InitializeComponent();

		_authService = authService;
	}

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await _authService.LogoutAsync();
    }
}