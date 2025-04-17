using System.Threading.Tasks;

namespace Momente
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void AddMomentButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MomentPage(0));
        }

        private void SwitchThemeButton_Clicked(object sender, EventArgs e)
        {
            AppTheme theme = Application.Current!.UserAppTheme == AppTheme.Dark ? AppTheme.Light : AppTheme.Dark;
            Application.Current!.UserAppTheme = theme;
            Preferences.Set("Theme", (int)theme);
            SwitchThemeButton.Text = Application.Current!.UserAppTheme == AppTheme.Dark ? "Mond" : "Sonne";
        }

        private void QuitButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}
