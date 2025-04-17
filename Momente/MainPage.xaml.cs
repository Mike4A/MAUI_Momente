using System.Threading.Tasks;

namespace Momente
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.NavigatedTo += MainPage_NavigatedTo;
            SwitchThemeButton.Text = Application.Current!.UserAppTheme == AppTheme.Dark ? "🌛" : "🌞";
        }

        private async void MainPage_NavigatedTo(object? sender, NavigatedToEventArgs e)
        {
            (BindingContext as MainViewModel)!.Moments!.Clear();
            List<Moment> moments = await DatabaseService.Instance.GetMomentsAsync();
            foreach (Moment moment in moments) {
                (BindingContext as MainViewModel)!.Moments!.Add(moment); 
            }            
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

            SwitchThemeButton.Text = Application.Current!.UserAppTheme == AppTheme.Dark ? "🌛" : "🌞";
        }

        private void QuitButton_Clicked(object sender, EventArgs e)
        {
            Application.Current!.Quit();
        }

        private async void MomentsCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MomentsCollectionView.SelectedItem != null)
            {
                int id = (MomentsCollectionView.SelectedItem as Moment)!.Id;
                await Navigation.PushAsync(new MomentPage(id));
            }            
        }
    }
}
