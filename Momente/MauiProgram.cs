using Microsoft.Extensions.Logging;

namespace Momente
{
    public static class MauiProgram
    {
        public const float MOMENT_COLOR_VALUE = 0.74f;

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
