using GoMeowee.Models.Interests;
using GoMeowee.Services;
using GoMeowee.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GoMeowee.ViewModels;

[QueryProperty(nameof(EventId), "EventId")]
public partial class EventDetailsViewModel : BaseViewModel
{
    private readonly EventService _eventsService;
    private readonly IAuthState _authState;
    private readonly HttpClient _httpClient;

    public Guid EventId { get; set { field = value; _ = LoadAsync(); } }

    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public string? ImageUrl { get; private set; }
    public string? FullImageUrl { get; private set; } 
    public DateTime StartsAt { get; private set; }
    public int InterestedCount { get; private set; }
    public bool IsUserInterested { get; private set; }
    public ObservableCollection<EventInterestDto> InterestedPeople { get; private set; } = [];
    public ICommand ToggleInterestCommand { get; }

    public EventDetailsViewModel(EventService eventsService, IAuthState authState, HttpClient httpClient)
    {
        _eventsService = eventsService;
        _authState = authState;
        _httpClient = httpClient;
        ToggleInterestCommand = new Command(async () => await ToggleInterestAsync());
    }

    private async Task LoadAsync()
    {
        if (IsBusy || EventId == Guid.Empty)
            return;

        try
        {
            IsBusy = true;

            var ev = await _eventsService.GetEventByIdAsync(EventId);
            var interested = await _eventsService.GetEventInterestByIdAsync(EventId);

            if (ev is null)
                return;

            Title = ev.Title;
            Description = ev.Description;
            Location = ev.Location;
            Category = ev.Category;
            ImageUrl = ev.ImageUrl;
            StartsAt = ev.StartsAt;
            InterestedCount = ev.InterestedCount;
            IsUserInterested = interested;

            FullImageUrl = _httpClient.BaseAddress + ImageUrl;

            OnPropertyChanged(string.Empty);

            InterestedPeople.Clear();

            var interests = await _eventsService.GetInterestedPeopleAsync(EventId);

            if (interests is not null)
                foreach (var person in interests)
                {
                    person.FullAvatarUrl = person.AvatarUrl is not null ? _httpClient.BaseAddress + person.AvatarUrl : null;
                    person.InterestedAtRelative = GetRelativeTime(person.InterestedAt);
                    InterestedPeople.Add(person);
                }
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task ToggleInterestAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            if (IsUserInterested)
            {
                if (await _eventsService.RemoveInterestAsync(EventId))
                {
                    IsUserInterested = false;
                    InterestedCount--;

                    var myInterest = InterestedPeople.FirstOrDefault(ip => ip.Username == _authState.CurrentUser!.Username);
                    if (myInterest is not null)
                        InterestedPeople.Remove(myInterest);
                }
            }
            else if (!IsUserInterested)
            {
                string? message = await Shell.Current.DisplayPromptAsync("Add a message", "", "OK", "Cancel", "(optional)");
                if (await _eventsService.SignalInterestAsync(EventId, message))
                {
                    IsUserInterested = true;
                    InterestedCount++;

                    var myInterest = new EventInterestDto()
                    {
                        FullAvatarUrl = _httpClient.BaseAddress + _authState.CurrentUser!.AvatarUrl,
                        Username = _authState.CurrentUser.Username,
                        DisplayName = _authState.CurrentUser.DisplayName,
                        InterestedAt = DateTime.Now,
                        InterestedAtRelative = "now",
                        Message = message
                    };
                    InterestedPeople.Insert(0, myInterest);
                }
            }

            OnPropertyChanged(nameof(IsUserInterested));
            OnPropertyChanged(nameof(InterestedCount));
        }
        finally
        {
            IsBusy = false;
        }
    }

    private static string GetRelativeTime(DateTime dateTime)
    {
        var span = DateTime.Now - dateTime;

        if (span.TotalSeconds < 60)
            return $"{(int)span.TotalSeconds}s ago";
        if (span.TotalMinutes < 60)
            return $"{(int)span.TotalMinutes}m ago";
        if (span.TotalHours < 24)
            return $"{(int)span.TotalHours}h ago";
        if (span.TotalDays < 7)
            return $"{(int)span.TotalDays}d ago";

        return $"{(int)(span.TotalDays / 7)}w ago";
    }
}