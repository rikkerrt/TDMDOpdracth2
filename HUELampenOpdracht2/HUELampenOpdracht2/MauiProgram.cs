using HUELampenOpdracht2.HUELampen.Domain.Models;
using HUELampenOpdracht2.HUELampen.Infrastructure;
using Microsoft.Extensions.Logging;
using HUELampenOpdracht2.HUELampen.ViewModel;


namespace HUELampenOpdracht2
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

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<HUEappViewModel>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<FetchUsername>();
            builder.Services.AddSingleton<HttpClient>();

            builder.Services.AddSingleton<IPreferences>(p => Preferences.Default);
            builder.Services.AddSingleton<IBridgeConnector, HueBridgeConnector>();

            return builder.Build();
        }
    }
}
