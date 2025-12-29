using GoMeowee.ViewModels;

namespace GoMeowee.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
    }
}