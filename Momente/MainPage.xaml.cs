using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using static Microsoft.Maui.Controls.VisualStateManager;

namespace Momente
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigatedTo += MainPage_NavigatedTo;
        }

        private void MainPage_NavigatedTo(object? sender, NavigatedToEventArgs e)
        {
            PopulateMomentsView();
        }
        private async void PopulateMomentsView()
        {
            if (DatabaseService.Instance.FilterCsv != null)
            {
                PopulatedMomentsViewFiltered();
                return;
            }
            ObservableCollection<Moment> moments = (BindingContext as MainViewModel)!.Moments!;
            Moment? selectedMoment = (MomentsCollectionView.SelectedItem as Moment);
            if (selectedMoment != null)
            {
                //Handle updated or deleted moment
                int selectedIndex = moments.IndexOf(selectedMoment);
                if (_momentPageArgs.Action is MomentAction.Deleted or MomentAction.Updated)
                {
                    moments.Remove(selectedMoment!);
                }
                if (_momentPageArgs.Action == MomentAction.Updated)
                {
                    Moment? updatedMoment = await DatabaseService.Instance.GetMomentByIdAsync(selectedMoment.Id);
                    if (updatedMoment != null)
                    {
                        moments.Insert(selectedIndex, updatedMoment);
                        MomentsCollectionView.ScrollTo(selectedMoment);
                    }
                }
                MomentsCollectionView.SelectedItem = null;
            }
            else
            {
                //Populate list if empty or add created moment                
                if (moments.Count == 0 || _momentPageArgs.Action == MomentAction.Created)
                {
                    Moment? lastMoment = await DatabaseService.Instance.GetLastMomentAsync();
                    if (lastMoment != null)
                    {
                        (BindingContext as MainViewModel)!.Moments!.Insert(0, lastMoment);
                        MomentsCollectionView.ScrollTo(lastMoment);
                    }
                }
            }
            _momentPageArgs = new MomentPageArgs();
        }
        private async void PopulatedMomentsViewFiltered()
        {
            List<Moment> filteredMoments = await DatabaseService.Instance.GetMomentsFilteredAndReversedAsync();
            (BindingContext as MainViewModel)!.Moments!.Clear();
            foreach (Moment filteredMoment in filteredMoments)
            {
                (BindingContext as MainViewModel)!.Moments!.Add(filteredMoment);
            }
            DatabaseService.Instance.ResetIdCounter();
        }

        private async void MomentsCollectionView_RemainingItemsThresholdReached(object sender, EventArgs e)
        {
            Moment? previousMoment = await DatabaseService.Instance.GetPreviousMomentAsync();
            if (previousMoment != null)
            {
                (BindingContext as MainViewModel)!.Moments!.Add(previousMoment);
            }
        }

        private async void QuitButton_Clicked(object sender, EventArgs e)
        {
            await QuitButton.ScaleTo(0.75, 50);
            await QuitButton.RotateXTo(180, 100);
            await QuitButton.RotateXTo(0, 100);
            await QuitButton.ScaleTo(1, 50);
            Application.Current!.Quit();
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
#if DEBUG
            _ = Debugger.WriteMomentEntries();
#endif
        }

        private async void SearchMomentsButton_Clicked(object sender, EventArgs e)
        {
            await SearchMomentsButton.ScaleTo(0.75, 50);
            await SearchMomentsButton.RotateXTo(180, 100);
            await SearchMomentsButton.RotateXTo(0, 100);
            await SearchMomentsButton.ScaleTo(1, 50);
            DatabaseService.Instance.FilterCsv = "";
            PopulateMomentsView();
        }

        private async void AddMomentButton_Clicked(object sender, EventArgs e)
        {
            await AddMomentButton.ScaleTo(0.75, 50);
            await AddMomentButton.RotateXTo(180, 100);
            await AddMomentButton.RotateXTo(0, 100);
            await AddMomentButton.ScaleTo(1, 50);
            MomentsCollectionView.SelectedItem = null;
            _momentPageArgs = (new MomentPageArgs());
            await Navigation.PushAsync(new MomentPage(_momentPageArgs));
        }

        private MomentPageArgs _momentPageArgs = new();
        private async void MomentsCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MomentsCollectionView.SelectedItem != null)
            {
                Moment selectedMoment = (MomentsCollectionView.SelectedItem as Moment)!;
                if (selectedMoment.Headline == "DevCheat" && selectedMoment.Color.ToHex() == "#000000")
                {
                    await DisplayAlert("DB-Path:", Path.Combine(FileSystem.AppDataDirectory, "moments.db"), "Done");
                }
                _momentPageArgs = new MomentPageArgs(selectedMoment);
                await Navigation.PushAsync(new MomentPage(_momentPageArgs));
            }
        }
    }
}
