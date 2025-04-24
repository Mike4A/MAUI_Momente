using Microsoft.Extensions.Logging;

namespace Momente
{
    public static class MauiProgram
    {
        public static Color DEFAULT_MOMENT_COLOR { get; } = Color.FromRgb(127, 128, 128);
        public static float LUMINOSITY_GLOW { get; } = 0.2f;

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
            await DatabaseService.Instance.TryAddWelcomeMomentAsync();
        }
    }
}
