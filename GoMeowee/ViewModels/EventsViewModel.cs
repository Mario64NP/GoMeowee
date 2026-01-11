using GoMeowee.Models;
using GoMeowee.Services;
using GoMeowee.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GoMeowee.ViewModels;

public partial class EventsViewModel : BaseViewModel
{
    private readonly EventService _eventsService;
    private readonly HttpClient _httpClient;

    public ObservableCollection<EventsDayGroup> EventsByDay { get; } = [];
    public ICommand OpenEventCommand { get; }

    public EventsViewModel(EventService eventsService, HttpClient httpClient)
    {
        _eventsService = eventsService;
        _httpClient = httpClient;
        OpenEventCommand = new Command<EventListItemDto>(OpenEvent);
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
                await Shell.Current.DisplayAlertAsync("Error", "Failed to load events", "OK");
                return;
            }

            foreach (var e in events)
                e.FullImageUrl = e.ImageUrl is not null ? _httpClient.BaseAddress + e.ImageUrl : null;

            var grouped = events
                .OrderBy(e => e.StartsAt)
                .GroupBy(e => e.StartsAt.Date)
                .Select(g => new EventsDayGroup(g.Key, g))
                .ToList();

            EventsByDay.Clear();
            foreach (var group in grouped)
                EventsByDay.Add(group);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async void OpenEvent(EventListItemDto ev)
    {
        if (ev == null)
            return;

        await Shell.Current.GoToAsync(
            $"{nameof(EventDetailsPage)}?EventId={ev.Id}");
    }
}
