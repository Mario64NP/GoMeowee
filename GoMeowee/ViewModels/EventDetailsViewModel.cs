using GoMeowee.Models;
using GoMeowee.Services;
using GoMeowee.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GoMeowee.ViewModels;

[QueryProperty(nameof(EventId), "EventId")]
public partial class EventDetailsViewModel : BaseViewModel
{
    private readonly EventsService _eventsService;
    private readonly IAuthState _authState;

    public Guid EventId
    {
        get => _eventId;
        set
        {
            _eventId = value;
            _ = LoadAsync();
        }
    }
    private Guid _eventId;

    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;
    public string? ImageUrl { get; private set; }
    public DateTime StartsAt { get; private set; }
    public int InterestedCount { get; private set; }
    public bool IsUserInterested { get; private set; }
    public ObservableCollection<UserInterestDto> InterestedPeople { get; private set; } = [];
    public ICommand ToggleInterestCommand { get; }

    public EventDetailsViewModel(EventsService eventsService, IAuthState authState)
    {
        _eventsService = eventsService;
        _authState = authState;
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
            ImageUrl = ev.ImageUrl;
            StartsAt = ev.StartsAt;
            InterestedCount = ev.InterestedCount;
            IsUserInterested = interested;

            OnPropertyChanged(string.Empty);

            InterestedPeople.Clear();

            var interests = await _eventsService.GetInterestedPeopleAsync(EventId);

            if (interests is not null)
                foreach (var person in interests)
                    InterestedPeople.Add(person);
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
                if (await _eventsService.SignalInterestAsync(EventId, null)) // change message
                {
                    IsUserInterested = true;
                    InterestedCount++;

                    var myInterest = new UserInterestDto()
                    {
                        AvatarUrl = _authState.CurrentUser!.AvatarUrl,
                        Username = _authState.CurrentUser.Username,
                        DisplayName = _authState.CurrentUser.DisplayName,
                        InterestedAt = DateTime.Now,
                        Message = "Test message"
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
}