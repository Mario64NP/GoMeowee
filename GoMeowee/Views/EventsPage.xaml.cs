using GoMeowee.Models.Events;
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

    private async void OnEventSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not EventListItemDto selected)
            return;

        ((CollectionView)sender).SelectedItem = null;

        await Shell.Current.GoToAsync(
            nameof(EventDetailsPage),
            new Dictionary<string, object>
            {
                ["EventId"] = selected.Id
            });
    }
}