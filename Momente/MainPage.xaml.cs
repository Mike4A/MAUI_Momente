using Momente.Resources.Localizations;
using System.Collections.ObjectModel;

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
            ObservableCollection<Moment> moments = (BindingContext as MainViewModel)!.Moments!;
            Moment? selectedMoment = (MomentsCollectionView.SelectedItem as Moment);
            if (selectedMoment != null)
            {
                //Handle updated or deleted moment
                int selectedIndex = moments.IndexOf(selectedMoment);
                if (_momentPageArgs.Action is MomentAction.Deleted)
                {
                    moments.Remove(selectedMoment!);

                }
                if (_momentPageArgs.Action == MomentAction.Updated)

                {
                    moments.Remove(selectedMoment!);
                    //await Task.Delay(1000);
                    moments.Insert(selectedIndex, _momentPageArgs.Moment);                 
                    //await Task.Delay(333);
                    MomentsCollectionView.ScrollTo(_momentPageArgs.Moment);
                    //Moment? updatedMoment = await DatabaseService.Instance.GetMomentByIdAsync(selectedMoment.Id);
                    //if (updatedMoment != null)
                    //{
                    //    moments.Insert(selectedIndex, updatedMoment);
                    //    MomentsCollectionView.ScrollTo(updatedMoment);
                    //}
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
            DefaultControlsGrid.IsVisible = false;
            _searchIndex = -1;
            SearchControlsGrid.IsVisible = true;
            SearchEntry.Focus();
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
            if (!_isSearching && MomentsCollectionView.SelectedItem != null)
            {
                Moment selectedMoment = (MomentsCollectionView.SelectedItem as Moment)!;
                if (selectedMoment.Headline == MauiProgram.DEV_CHEAT_CODE && selectedMoment.Color.ToHex() == MauiProgram.DEV_CHEAT_COlOR)
                {
                    string msg =
                        $"DB-Path: {Path.Combine(FileSystem.AppDataDirectory)}\n" +
                        $"DB-Count: {await DatabaseService.Instance.GetCount()}";
                    await DisplayAlert("", msg, "Ok");
                }
                _momentPageArgs = new MomentPageArgs(selectedMoment);
                await Navigation.PushAsync(new MomentPage(_momentPageArgs));
            }
        }

        private int _firstVisibleIndex = 0;
        private int _lastVisibleIndex = 0;

        private void MomentsCollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            _firstVisibleIndex = e.FirstVisibleItemIndex;
            _lastVisibleIndex = e.LastVisibleItemIndex;
            if (!_isSearching)
            {
                _searchIndex = -1;
            }
        }

        private bool _isSearching = false;
        private int _searchIndex;
        private async void FindNextButton_Clicked(object sender, EventArgs e)
        {
            await FindNextButton.ScaleTo(0.75, 50);
            await FindNextButton.RotateXTo(180, 100);
            await FindNextButton.RotateXTo(0, 100);
            await FindNextButton.ScaleTo(1, 50);
            if (_isSearching || String.IsNullOrEmpty(SearchEntry.Text)) { return; }
            SearchEntry.IsEnabled = false;
            SearchEntry.IsEnabled = true;
            _isSearching = true;
            ObservableCollection<Moment> moments = (BindingContext as MainViewModel)!.Moments!;
            if (_searchIndex == -1) { _searchIndex = _lastVisibleIndex + 1; }
            do
            {
                _searchIndex--;
            } while (_searchIndex > -1 && !MomentMatchesSearchPatter(moments[_searchIndex]));
            if (_searchIndex == -1)
            {
                AlertSearchReachedEnd();
                _isSearching = false;
            }
            else
            {
                ScrollToAndHighlight_FoundMoment(moments[_searchIndex]);
            }
        }
        private async void FindPreviousButton_Clicked(object sender, EventArgs e)
        {
            await FindPreviousButton.ScaleTo(0.75, 50);
            await FindPreviousButton.RotateXTo(180, 100);
            await FindPreviousButton.RotateXTo(0, 100);
            await FindPreviousButton.ScaleTo(1, 50);
            if (_isSearching || String.IsNullOrEmpty(SearchEntry.Text)) { return; }
            SearchEntry.IsEnabled = false;
            SearchEntry.IsEnabled = true;
            _isSearching = true;
            ObservableCollection<Moment> moments = (BindingContext as MainViewModel)!.Moments!;
            if (_searchIndex == -1) { _searchIndex = _firstVisibleIndex - 1; }
            do
            {
                _searchIndex++;
                if (_searchIndex > moments.Count - 1)
                {
                    Moment? previousMoment = await DatabaseService.Instance.GetPreviousMomentAsync();
                    if (previousMoment != null)
                    {
                        (BindingContext as MainViewModel)!.Moments!.Add(previousMoment);                        
                    }
                    else
                    {
                        _searchIndex = -1;
                        break;
                    }
                }
            } while (!MomentMatchesSearchPatter(moments[_searchIndex]));
            if (_searchIndex == -1)
            {
                AlertSearchReachedEnd();
                _isSearching = false;
            }
            else
            {
                ScrollToAndHighlight_FoundMoment(moments[_searchIndex]);
            }
        }
        private async void AlertSearchReachedEnd()
        {
            await DisplayAlert("", AppResources.SearchMsgNoMoreResults, "Ok");
        }
        private async void ScrollToAndHighlight_FoundMoment(Moment moment)
        {
            MomentsCollectionView.ScrollTo(moment, ScrollToPosition.End);
            await Task.Delay(1000);
            MomentsCollectionView.SelectedItem = moment;
            await Task.Delay(200);
            MomentsCollectionView.SelectedItem = null;
            await Task.Delay(200);
            MomentsCollectionView.SelectedItem = moment;
            await Task.Delay(200);
            MomentsCollectionView.SelectedItem = null;
            await Task.Delay(200);
            MomentsCollectionView.SelectedItem = moment;
            await Task.Delay(200);
            MomentsCollectionView.SelectedItem = null;
            _isSearching = false;
        }
        private bool MomentMatchesSearchPatter(Moment moment)
        {
            string[] filters = SearchEntry.Text.Split(",");
            foreach (string filter in filters)
            {
                if (!string.IsNullOrEmpty(filter))
                {
                    if (moment.Icon.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                        moment.CreatedAtString.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                        moment.Headline.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                        moment.Description.Contains(filter, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private async void CancelSearchButton_Clicked(object sender, EventArgs e)
        {
            await CancelSearchButton.ScaleTo(0.75, 50);
            await CancelSearchButton.RotateXTo(180, 100);
            await CancelSearchButton.RotateXTo(0, 100);
            await CancelSearchButton.ScaleTo(1, 50);
            SearchControlsGrid.IsVisible = false;
            DefaultControlsGrid.IsVisible = true;
            SearchEntry.IsEnabled = false;
            SearchEntry.IsEnabled = true;
        }

        private void SearchEntry_Focused(object sender, FocusEventArgs e)
        {
            Dispatcher.Dispatch(() =>
            {
                if (!String.IsNullOrEmpty(SearchEntry.Text))
                {
                    SearchEntry.CursorPosition = 0;
                    SearchEntry.SelectionLength = SearchEntry.Text.Length;
                }
            });
        }

        private void SearchEntry_Unfocused(object sender, FocusEventArgs e)
        {
            SearchEntry.IsEnabled = false;
            SearchEntry.IsEnabled = true;
        }
    }
}
