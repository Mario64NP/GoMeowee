using GoMeowee.ViewModels;

namespace GoMeowee.Views;

public partial class EventDetailsPage : ContentPage
{
	public EventDetailsPage(EventDetailsViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}