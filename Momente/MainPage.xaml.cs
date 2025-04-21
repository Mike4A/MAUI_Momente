using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Momente
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigatedTo += MainPage_NavigatedTo;
            SwitchThemeButton.Text = Application.Current!.UserAppTheme == AppTheme.Dark ? "🌛" : "🌞";
        }

        private async void MainPage_NavigatedTo(object? sender, NavigatedToEventArgs e)
        {
            ObservableCollection<Moment> moments = (BindingContext as MainViewModel)!.Moments!;
            if (MomentsCollectionView.SelectedItem == null)
            {
                //Populate list if empty or handle added moment
                if (moments.Count == 0 || _momentPageArgs.Action == MomentAction.Saved)
                {
                    Moment lastMoment = await DatabaseService.Instance.GetLastMomentAsync();
                    if (lastMoment != null && (moments.Count == 0 || moments[0].Id != lastMoment.Id))
                    {
                        (BindingContext as MainViewModel)!.Moments!.Insert(0, lastMoment);
                        MomentsCollectionView.ScrollTo(lastMoment);
                    }
                }
            }
            else
            {
                //Handle updated or deleted moment
                Moment selectedMoment = (MomentsCollectionView.SelectedItem as Moment)!;
                int selectedIndex = moments.IndexOf(selectedMoment);
                MomentsCollectionView.SelectedItem = null;
                if (_momentPageArgs.Action is MomentAction.Deleted or MomentAction.Saved)
                {
                    moments.Remove(selectedMoment);
                }
                if (_momentPageArgs.Action == MomentAction.Saved)
                {
                    moments.Insert(selectedIndex, selectedMoment);
                    MomentsCollectionView.ScrollTo(selectedMoment);
                }
            }
            _momentPageArgs = new MomentPageArgs();
        }

        private async void MomentsCollectionView_RemainingItemsThresholdReached(object sender, EventArgs e)
        {
            Moment previousMoment = await DatabaseService.Instance.GetPreviousMomentAsync();
            if (previousMoment != null)
            {
                (BindingContext as MainViewModel)!.Moments!.Add(previousMoment);
            }
        }

        private async void SwitchThemeButton_Clicked(object sender, EventArgs e)
        {
            await SwitchThemeButton.ScaleTo(0.75, 50);
            await SwitchThemeButton.RotateXTo(180, 100);
            await SwitchThemeButton.RotateXTo(0, 100);
            await SwitchThemeButton.ScaleTo(1, 50);
            AppTheme theme = Application.Current!.UserAppTheme == AppTheme.Dark ? AppTheme.Light : AppTheme.Dark;
            Application.Current!.UserAppTheme = theme;
            Preferences.Set("Theme", (int)theme);
            SwitchThemeButton.Text = Application.Current!.UserAppTheme == AppTheme.Dark ? "🌛" : "🌞";
#if DEBUG
            _ = Debugger.WriteMomentEntries();
#endif
        }

        private async void QuitButton_Clicked(object sender, EventArgs e)
        {
            await QuitButton.ScaleTo(0.75, 50);
            await QuitButton.RotateXTo(180, 100);
            await QuitButton.RotateXTo(0, 100);
            await QuitButton.ScaleTo(1, 50);
            Application.Current!.Quit();
        }

        private async void AddMomentButton_Clicked(object sender, EventArgs e)
        {
            await AddMomentButton.ScaleTo(0.75, 50);
            await AddMomentButton.RotateXTo(180, 100);
            await AddMomentButton.RotateXTo(0, 100);
            await AddMomentButton.ScaleTo(1, 50);
            MomentsCollectionView.SelectedItem = null;
            _momentPageArgs = (new MomentPageArgs());
            await Navigation.PushModalAsync(new MomentPage(_momentPageArgs));
        }

        private MomentPageArgs _momentPageArgs = new();
        private async void MomentsCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MomentsCollectionView.SelectedItem != null)
            {
                _momentPageArgs = (new MomentPageArgs((MomentsCollectionView.SelectedItem as Moment)!));
                await Navigation.PushModalAsync(new MomentPage(_momentPageArgs));
            }
        }
    }
}
