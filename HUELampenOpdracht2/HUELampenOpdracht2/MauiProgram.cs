using Microsoft.Extensions.Logging;

using HUELampen.Domain.ClientInterface;
using HUELampen.Infrastructure.BridgeConnection;


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
            builder.Services.AddHttpClient<IHTTPClient, BridgeConnection>(o =>
            {
                o.BaseAddress = new Uri("http://localhost/api/newdeveloper");
                o.Timeout = TimeSpan.FromSeconds(3);
            });
#endif

            return builder.Build();
        }
    }
}
