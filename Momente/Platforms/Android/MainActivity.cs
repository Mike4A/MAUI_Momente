using Android.App;
using Android.Content.PM;
using Android.OS;

namespace Momente
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            Window!.SetStatusBarColor(Android.Graphics.Color.DarkCyan);
            Window!.SetNavigationBarColor(Android.Graphics.Color.DarkCyan);
            base.OnCreate(savedInstanceState);
        }
    }
}
