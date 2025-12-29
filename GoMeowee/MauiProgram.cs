using GoMeowee.Shells;
using GoMeowee.ViewModels;
using GoMeowee.Views;
﻿using Microsoft.Extensions.Logging;

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

            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<AuthShell>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<EventsViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<EventsPage>();
#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
