namespace Momente
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            AppTheme theme = (AppTheme)Preferences.Get("Theme", (int)Application.Current!.RequestedTheme);
            Application.Current!.UserAppTheme = theme;            

            MainPage = new AppShell();
        }
    }
}
