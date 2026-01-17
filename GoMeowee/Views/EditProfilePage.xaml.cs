using GoMeowee.ViewModels;

namespace GoMeowee.Views;

public partial class EditProfilePage : ContentPage
{
	private readonly EditProfileViewModel _viewModel;
	public EditProfilePage(EditProfileViewModel viewModel)
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