using Microsoft.Extensions.Logging;
using Momente.Models;
using Momente.Services;

namespace Momente
{
    //v0.13
    public static class MauiProgram
    {
        public static Color MOMENT_DEFAULT_COLOR { get; } = Color.FromRgb(127, 128, 128);

        public const float MOMENT_LUMINOSITY_SHADOW = -0.1f;
        public const float MOMENT_LUMINOSITY_GLOW = 0.1f;
        public const string DEV_CHEAT_CODE = "DevCheat";
        public const string DEV_CHEAT_COlOR = "#000000";
        public const int SEARCH_PROMPT_LIMIT = 100;
        public const string DATE_FORMAT_STRING = "ddd. dd. MMMM yyyy, HH:mm";

        public static MauiApp CreateMauiApp()
        {
            TryToAddWelcomeEntry();
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
            return builder.Build();
        }

        private static async void TryToAddWelcomeEntry()
        {
            if (await DatabaseService.Instance.GetCount() == 0)
            {
                await DatabaseService.Instance.AddMomentAsync(Moment.CreateWelcomeMoment());
                while (DatabaseService.Instance.GetLastMomentAsync() == null)
                { await Task.Delay(500); }
            }
        }
    }
}
