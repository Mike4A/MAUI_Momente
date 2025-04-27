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
            //if (DatabaseService.Instance.FilterCsv != null)
            //{
            //    PopulatedMomentsViewFiltered();
            //    return;
            //}
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
                        MomentsCollectionView.ScrollTo(updatedMoment);
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
        //private async void PopulatedMomentsViewFiltered()
        //{
        //    (BindingContext as MainViewModel)!.Moments!.Clear();
        //    var moments = (BindingContext as MainViewModel)!.Moments!;
        //    List<Moment> filteredMoments = await DatabaseService.Instance.GetMomentsFilteredAndReversedAsync();
        //    foreach (Moment filteredMoment in filteredMoments)
        //    {
        //        (BindingContext as MainViewModel)!.Moments!.Add(filteredMoment);
        //    }
        //}

        private async void MomentsCollectionView_RemainingItemsThresholdReached(object sender, EventArgs e)
        {
            //if (!String.IsNullOrEmpty(DatabaseService.Instance.FilterCsv))
            //{ return; }
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
            //if (SearchMomentsButton.Text == "🔎")
            //{
            //    string filter = await DisplayPromptAsync("", "Suchen nach?", "Ok", "Abbrechen", "...");
            //    if (!string.IsNullOrEmpty(filter))
            //    {
            //        DatabaseService.Instance.FilterCsv = filter;
            //        SearchMomentsButton.Text = "🔎❌";
            //        PopulateMomentsView();
            //    }
            //}
            //else
            //{
            //    DatabaseService.Instance.FilterCsv = null;
            //    DatabaseService.Instance.ResetIdCounter();
            //    SearchMomentsButton.Text = "🔎";
            //    (BindingContext as MainViewModel)!.Moments!.Clear();
            //    MomentsCollectionView.SelectedItem = null;
            //    PopulateMomentsView();
            //}
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
        private void FindNextButton_Clicked(object sender, EventArgs e)
        {
            if (_isSearching) { return; }
            SearchEntry.Unfocus();
            _isSearching = true;
            ObservableCollection<Moment> moments = (BindingContext as MainViewModel)!.Moments!;
            if (String.IsNullOrEmpty(SearchEntry.Text)) { return; }
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
                HighlightFoundMoment(moments[_searchIndex]);
            }
        }
        private async void FindPreviousButton_Clicked(object sender, EventArgs e)
        {
            if (_isSearching) { return; }
            SearchEntry.Unfocus();
            _isSearching = true;
            ObservableCollection<Moment> moments = (BindingContext as MainViewModel)!.Moments!;
            if (String.IsNullOrEmpty(SearchEntry.Text)) { return; }
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
                HighlightFoundMoment(moments[_searchIndex]);
            }
        }
        private async void AlertSearchReachedEnd()
        {
            await DisplayAlert("", AppResources.SearchMsgNoMoreResults, "Ok");
        }
        private async void HighlightFoundMoment(Moment moment)
        {
            MomentsCollectionView.ScrollTo(moment, ScrollToPosition.End);    // Code to run on the main thread
            await Task.Delay(500);
            MomentsCollectionView.SelectedItem = moment;
            await Task.Delay(500);
            MomentsCollectionView.SelectedItem = null;
            await Task.Delay(500);
            MomentsCollectionView.SelectedItem = moment;
            await Task.Delay(500);
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

        private void CancelSearchButton_Clicked(object sender, EventArgs e)
        {
            SearchControlsGrid.IsVisible = false;
            DefaultControlsGrid.IsVisible = true;
        }

        private void SearchEntry_TextChanged(object sender, TextChangedEventArgs e)
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
    }
}
