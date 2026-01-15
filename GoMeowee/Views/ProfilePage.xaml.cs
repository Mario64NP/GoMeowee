using GoMeowee.ViewModels;

namespace GoMeowee.Views;

public partial class ProfilePage : ContentPage
{
    private readonly ProfilePageViewModel _viewModel;
	public ProfilePage(ProfilePageViewModel viewModel)
	{
		InitializeComponent();
        
        BindingContext = viewModel;
        _viewModel = viewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.OnAppearing();
    }
}