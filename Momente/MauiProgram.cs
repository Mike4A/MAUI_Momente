using Microsoft.Extensions.Logging;

namespace Momente
{
    //v0.10
    public static class MauiProgram
    {
        public static Color MOMENT_DEFAULT_COLOR { get; } = Color.FromRgb(127, 128, 128);
        public const float MOMENT_LUMINOSITY_GLOW = 0.2f;
        public const string DEV_CHEAT_CODE  = "DevCheat";
        public const string DEV_CHEAT_COlOR = "#000000";        

        public static MauiApp CreateMauiApp()
        {
            AddWelcomeEntry();
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

        private static async void AddWelcomeEntry()
        {
            await DatabaseService.Instance.AddWelcomeMomentIfEmptyAsync();
            Thread.Sleep(1000);
        }
    }
}
