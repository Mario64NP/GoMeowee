using GoMeowee.Models;
using GoMeowee.Services;
using GoMeowee.Views;
using System.Collections.ObjectModel;

namespace GoMeowee.ViewModels;

public partial class EventsViewModel : BaseViewModel
{
    private readonly EventsService _eventsService;

    public ObservableCollection<EventsDayGroup> EventsByDay { get; } = [];

    public EventsViewModel(EventsService eventsService)
    {
        _eventsService = eventsService;
    }

    public async Task LoadAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            var events = await _eventsService.GetEventsAsync();

            if (events is null)
            {
                //pop-up message saying failed to load
                await Shell.Current.DisplayAlertAsync("Error", "Failed to load events", "OK");
                return;
            }

            var grouped = events
                .OrderBy(e => e.StartsAt)
                .GroupBy(e => e.StartsAt.Date)
                .Select(g => new EventsDayGroup(g.Key, g));

            EventsByDay.Clear();
            foreach (var group in grouped)
                EventsByDay.Add(group);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
