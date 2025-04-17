using System.Threading.Tasks;

namespace Momente
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.NavigatedTo += MainPage_NavigatedTo;
        }

        private async void MainPage_NavigatedTo(object? sender, NavigatedToEventArgs e)
        {
            await AddNextMoment();
        }
        private async void MomentsCollectionView_RemainingItemsThresholdReached(object sender, EventArgs e)
        {
            await AddNextMoment();
        }
        private async Task AddNextMoment()
        {
            Moment? moment = await DatabaseService.Instance.GetNextCachedAsync();

            if (moment != null)
            {
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
                (BindingContext as MainViewModel)!.Moments!.Clear();
            }            
        }
    }
}
