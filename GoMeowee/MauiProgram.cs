using GoMeowee.Services;
using GoMeowee.Services.Interfaces;
using GoMeowee.Shells;
using GoMeowee.ViewModels;
using GoMeowee.Views;
using Microsoft.Extensions.Logging;

[assembly: XmlnsDefinition("http://schemas.gomeowee.com/models", "GoMeowee.Models")]
[assembly: XmlnsDefinition("http://schemas.gomeowee.com/models", "GoMeowee.Models.Auth")]
[assembly: XmlnsDefinition("http://schemas.gomeowee.com/models", "GoMeowee.Models.Users")]
[assembly: XmlnsDefinition("http://schemas.gomeowee.com/models", "GoMeowee.Models.Events")]
[assembly: XmlnsDefinition("http://schemas.gomeowee.com/models", "GoMeowee.Models.Interests")]

namespace GoMeowee
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("SegoeFluentIcons.ttf", "SegoeFluentIcons");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton(new HttpClient()
            {
                BaseAddress = new Uri("https://z90wj05w-7271.euw.devtunnels.ms/")
            });

            builder.Services.AddSingleton<ApiClient>();

            builder.Services.AddSingleton<IAuthState, AuthState>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IAuthStorage, AuthStorage>();

            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<AuthShell>();

            builder.Services.AddSingleton<EventService>();
            builder.Services.AddSingleton<UserService>();

            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<EventsViewModel>();
            builder.Services.AddTransient<EventDetailsViewModel>();
            builder.Services.AddTransient<ProfilePageViewModel>();
            builder.Services.AddTransient<EditProfileViewModel>();
            builder.Services.AddTransient<SettingsViewModel>();

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<EventsPage>();
            builder.Services.AddTransient<EventDetailsPage>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<EditProfilePage>();
            builder.Services.AddTransient<SettingsPage>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
