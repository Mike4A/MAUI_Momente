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
            //Handle deleted and updated moments in collection view
            if (MomentsCollectionView.SelectedItem != null)
            {
                Moment selectedMoment = (MomentsCollectionView.SelectedItem as Moment)!;
                int selectedIndex = moments.IndexOf(selectedMoment);
                //remove happens in both cases: When moment was deleted and when moment was updated
                moments.Remove(selectedMoment);
                //Insert moment back into collection if it wasn't deleted to update collection view                
                if (await DatabaseService.Instance.GetMomentByIdAsync(selectedMoment.Id) != null)
                {
                    moments.Insert(selectedIndex, selectedMoment);
                    MomentsCollectionView.ScrollTo(selectedMoment);
                }
                MomentsCollectionView.SelectedItem = null;
            }
            //or try to add last added moment
            else
            {
                Moment lastMoment = await DatabaseService.Instance.GetLastMomentAsync();
                if (lastMoment != null && (moments.Count == 0 || moments[0].Id != lastMoment.Id))
                {
                    (BindingContext as MainViewModel)!.Moments!.Insert(0, lastMoment);
                    MomentsCollectionView.ScrollTo(lastMoment);
                }
            }
        }

        private async void MomentsCollectionView_RemainingItemsThresholdReached(object sender, EventArgs e)
        {
            Moment previousMoment = await DatabaseService.Instance.GetPreviousMomentAsync();
            if (previousMoment != null)
            {
                (BindingContext as MainViewModel)!.Moments!.Add(previousMoment);
            }
        }

        private async void AddMomentButton_Clicked(object sender, EventArgs e)
        {
            AnimationService.AnimateButton(AddMomentButton);
            MomentsCollectionView.SelectedItem = null;
            await Navigation.PushModalAsync(new MomentPage(new Moment()));
        }

        private void SwitchThemeButton_Clicked(object sender, EventArgs e)
        {
            AnimationService.AnimateButton(SwitchThemeButton);
            AppTheme theme = Application.Current!.UserAppTheme == AppTheme.Dark ? AppTheme.Light : AppTheme.Dark;
            Application.Current!.UserAppTheme = theme;
            Preferences.Set("Theme", (int)theme);
            SwitchThemeButton.Text = Application.Current!.UserAppTheme == AppTheme.Dark ? "🌛" : "🌞";
#if DEBUG
            _ = Debugger.WriteMomentEntries();
#endif
        }

        private void QuitButton_Clicked(object sender, EventArgs e)
        {
            AnimationService.AnimateButton(QuitButton);
            Application.Current!.Quit();
        }

        private async void MomentsCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MomentsCollectionView.SelectedItem != null)
            {
                await Navigation.PushModalAsync(new MomentPage((MomentsCollectionView.SelectedItem as Moment)!));
            }
        }
    }
}
