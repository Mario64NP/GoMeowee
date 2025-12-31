using GoMeowee.Services;
using GoMeowee.Services.Interfaces;
using GoMeowee.Shells;
using GoMeowee.ViewModels;
using GoMeowee.Views;

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
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            /*var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>  true 
            };*/

            builder.Services.AddSingleton(new HttpClient() /*handler)*/
            {
                BaseAddress = new Uri("https://z90wj05w-7271.euw.devtunnels.ms/")
            });

            builder.Services.AddSingleton<ApiClient>();

            builder.Services.AddSingleton<IAuthState, AuthState>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IAuthStorage, AuthStorage>();

            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<AuthShell>();

            builder.Services.AddSingleton<EventsService>();

            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<EventsViewModel>();
            builder.Services.AddTransient<EventDetailsViewModel>();

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<EventsPage>();
            builder.Services.AddTransient<EventDetailsPage>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
