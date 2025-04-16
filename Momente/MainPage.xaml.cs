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
            //testing
            await Navigation.PushAsync(new MomentPage(0, false));
        }

        private void SwitchThemeButton_Clicked(object sender, EventArgs e)
        {
            Application.Current!.UserAppTheme = Application.Current!.UserAppTheme == AppTheme.Dark ? AppTheme.Light : AppTheme.Dark;
        }
    }
}
