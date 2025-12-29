using GoMeowee.ViewModels;

namespace GoMeowee.Views;

public partial class EventsPage : ContentPage
{
    private readonly EventsViewModel _viewModel;
    public EventsPage(EventsViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadAsync();
    }
}