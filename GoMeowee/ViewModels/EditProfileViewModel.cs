using GoMeowee.Services;
using GoMeowee.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GoMeowee.ViewModels;

public partial class EditProfileViewModel : BaseViewModel
{
    private readonly UserService _userService;
    private readonly IAuthState _authState;
    private readonly HttpClient _httpClient;

    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? FullAvatarUrl { get; set; }
    public ObservableCollection<string> Tags { get; set; } = [];
    public ICommand GoBackCommand { get; } = new Command(async () => { await Shell.Current.GoToAsync(".."); });
    public ICommand DeleteTagCommand { get; }
    public ICommand UpdateAvatarCommand { get; }
    public ICommand SaveChangesCommand { get; }

    public EditProfileViewModel(UserService userService, IAuthState authState, HttpClient httpClient)
    {
        _userService = userService;
        _authState = authState;
        _httpClient = httpClient;

        DeleteTagCommand = new Command(async (tag) => await DeleteTag(tag));
        SaveChangesCommand = new Command(async () => await SaveChanges());
        UpdateAvatarCommand = new Command(async () => await UpdateAvatar());
    }

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

            Username = _authState.CurrentUser!.Username;
            DisplayName = _authState.CurrentUser.DisplayName;
            Bio = _authState.CurrentUser.Bio;
            FullAvatarUrl = _authState.CurrentUser.AvatarUrl is not null ? _httpClient.BaseAddress + _authState.CurrentUser.AvatarUrl : null;
            
            foreach (string tag in _authState.CurrentUser.Tags)
                Tags.Add(tag);

            OnPropertyChanged(string.Empty);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task DeleteTag(object tag)
    {
        if (tag is string t)
            Tags.Remove(t);
    }

    private async Task UpdateAvatar()
    {
        var photo = (await MediaPicker.Default.PickPhotosAsync(new MediaPickerOptions())).FirstOrDefault();

        if (photo is null)
            return;

        IsBusy = true;
        var result = await _userService.UploadAvatarAsync(photo);

        if (result is not null)
        {
            _authState.CurrentUser!.AvatarUrl = result ?? _authState.CurrentUser.AvatarUrl;
            FullAvatarUrl = _httpClient.BaseAddress + _authState.CurrentUser.AvatarUrl;
            OnPropertyChanged(nameof(FullAvatarUrl));
        }
    }

    private async Task SaveChanges()
    {
        var user = await _userService.UpdateUserAsync(Username, DisplayName, Bio, Tags);

        if (user is not null)
        {
            _authState.CurrentUser!.DisplayName = user.DisplayName;
            _authState.CurrentUser.Bio = user.Bio;
            _authState.CurrentUser.Tags = user.Tags;

            GoBackCommand.Execute(null);
        }
    }
}
