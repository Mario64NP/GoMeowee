using GoMeowee.Models;
using System.Collections.ObjectModel;

namespace GoMeowee.ViewModels;

public partial class EventsViewModel : BaseViewModel
{
    public ObservableCollection<EventsDayGroup> EventsByDay { get; } = [];

    public EventsViewModel()
    {

    }

    public async Task LoadAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
