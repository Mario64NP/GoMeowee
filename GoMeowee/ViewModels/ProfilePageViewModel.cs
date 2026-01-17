using GoMeowee.Models.Interests;
using GoMeowee.Models.Users;
using GoMeowee.Services;
using GoMeowee.Services.Interfaces;
using GoMeowee.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GoMeowee.ViewModels;

[QueryProperty(nameof(Username), "Username")]
public partial class ProfilePageViewModel(UserService userService, IAuthState authState, HttpClient httpClient) : BaseViewModel
{
    public Guid UserId { get; private set; }
    public bool IsMe { get; private set; }
    public string? Username { get; private set; }
    public string DisplayName { get; private set; } = string.Empty;
    public string? Bio { get; private set; }
    public string? FullAvatarUrl { get; private set; }
    public DateTime JoinedAt {  get; private set; }
    public bool HasInterests { get; private set; }
    public int InterestedEventsCount { get; private set; }
    public ObservableCollection<UserInterestDto> InterestedEvents { get; private set; } = [];
    public ICommand GoBackCommand { get; } = new Command(async () => { await Shell.Current.GoToAsync(".."); });
    public ICommand OpenSettingsPageCommand { get; } = new Command(async () => { await Shell.Current.GoToAsync(nameof(SettingsPage)); });
    public ICommand OpenProfileMenuCommand { get; } = new Command(async () => await OpenMenu());
    public ICommand EditProfileCommand { get; private set; } = new Command(async () => { await Shell.Current.GoToAsync(nameof(EditProfilePage)); });
    public ICommand SendMessageCommand { get; private set; } = new Command(async () => { await Shell.Current.GoToAsync(".."); }); //goto messages page

    public async Task OnAppearing()
    {
        if (IsBusy)
            return;

        await LoadAsync();
    }
    private async Task LoadAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            UserDetailsDto? targetUser;

            if (Username == authState.CurrentUser!.Username || string.IsNullOrEmpty(Username))
            {
                IsMe = true;
                targetUser = authState.CurrentUser;
            }
            else
            {
                IsMe = false;
                targetUser = await userService.GetUserByUsernameAsync(Username);
            }

            if (targetUser is null)
                return;

            UserId = targetUser.Id;
            Username = targetUser.Username;
            DisplayName = targetUser.DisplayName;
            FullAvatarUrl = httpClient.BaseAddress + targetUser.AvatarUrl;
            Bio = targetUser.Bio;

            OnPropertyChanged(string.Empty);

            InterestedEvents.Clear();

            var interests = await userService.GetInterestedEventsByUserAsync(Username);

            if (interests is not null)
                foreach (var interest in interests)
                {
                    interest.FullAvatarUrl = interest.AvatarUrl is not null ? httpClient.BaseAddress + interest.AvatarUrl : null;
                    interest.FullImageUrl  = interest.ImageUrl  is not null ? httpClient.BaseAddress + interest.ImageUrl  : null;
                    interest.InterestedAtRelative = GetRelativeTime(interest.InterestedAt);
                    InterestedEvents.Add(interest);
                }

            InterestedEventsCount = InterestedEvents.Count;
            HasInterests = InterestedEventsCount > 0;

            OnPropertyChanged(string.Empty);
        }
        finally
        {
            IsBusy = false;
        }
    }

    public static async Task OpenMenu()
    {
        string result = await Shell.Current.DisplayActionSheetAsync(
            "Options",
            "Cancel",
            "Block user",
            "Report", "Copy profile URL"
            );

        switch (result)
        {
            case "Block user":
                break;
            case "Report":
                break;
            case "Copy profile URL":
                break;

            default:
                break;
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
