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
            ObservableCollection<MomentItemViewModel> momentItems = (BindingContext as MainPageViewModel)!.MomentItems!;
            MomentItemViewModel? selectedMomentItem = MomentsCollectionView.SelectedItem as MomentItemViewModel;
            if (selectedMomentItem != null)
            {
                //Handle updated or deleted moment
                int selectedIndex = momentItems.IndexOf(selectedMomentItem);
                if (_momentPageArgs.Action is MomentAction.Deleted)
                {
                    momentItems.Remove(selectedMomentItem!);

                }
                if (_momentPageArgs.Action == MomentAction.Updated)

                {
                    MomentItemViewModel updatedMomentItem = new MomentItemViewModel(_momentPageArgs.Moment);
                    momentItems[selectedIndex] =updatedMomentItem;
                    MomentsCollectionView.ScrollTo(updatedMomentItem);
                }
                MomentsCollectionView.SelectedItem = null;
            }
            else
            {
                //Populate list if empty or add created moment                
                if (momentItems.Count == 0 || _momentPageArgs.Action == MomentAction.Created)
                {
                    Moment? lastMoment = await DatabaseService.Instance.GetLastMomentAsync();
                    if (lastMoment != null)
                    {
                        MomentItemViewModel lastMomentItem = new MomentItemViewModel(lastMoment);
                        (BindingContext as MainPageViewModel)!.MomentItems!.Insert(0, lastMomentItem);
                        var items = (BindingContext as MainPageViewModel)!.MomentItems!;
                        MomentsCollectionView.ScrollTo(lastMomentItem);
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
                (BindingContext as MainPageViewModel)!.MomentItems!.Add(new MomentItemViewModel(previousMoment));
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
                MomentItemViewModel selectedMomentItem = (MomentsCollectionView.SelectedItem as MomentItemViewModel)!;
                if (selectedMomentItem.Headline == MauiProgram.DEV_CHEAT_CODE && selectedMomentItem.Color!.ToHex() == MauiProgram.DEV_CHEAT_COlOR)
                {
                    string msg =
                        $"DB Path: {Path.Combine(FileSystem.AppDataDirectory)}\n" +
                        $"DB Count: {await DatabaseService.Instance.GetCount()}\n" +
                        $"Loaded Moments: {(BindingContext as MainPageViewModel)!.MomentItems!.Count}";
                    await DisplayAlert("", msg, "Ok");
                }
                _momentPageArgs = new MomentPageArgs((await DatabaseService.Instance.GetMomentByIdAsync(selectedMomentItem.Id))!);
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
                ObservableCollection<MomentItemViewModel> momenttems = (BindingContext as MainPageViewModel)!.MomentItems!;
                if (momenttems != null && momenttems.Count > 0)
                {
                    //Drop invisible items
                    for (int i = momenttems.Count - 1; i > _lastVisibleIndex + 3; i--)
                    {
                        momenttems.Remove(momenttems[i]);
                        DatabaseService.Instance.IdCounter = momenttems[i - 1].Id;
                    }
                }
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
            ObservableCollection<MomentItemViewModel> momentItems = (BindingContext as MainPageViewModel)!.MomentItems!;
            if (_searchIndex == -1) { _searchIndex = _lastVisibleIndex + 1; }
            int searched = 0;
            do
            {
                _searchIndex--;
                if (++searched % MauiProgram.SEARCH_PROMPT_LIMIT == 0)
                {
                    if (_searchIndex > -1)
                    {
                        MomentsCollectionView.ScrollTo(momentItems[_searchIndex], ScrollToPosition.MakeVisible);
                    }
                    bool result = await DisplayAlert("", AppResources.SearchLimitReachedText, AppResources.Yes, AppResources.No);
                    if (!result) { break; }
                }

            } while (_searchIndex > -1 && !MomentItemMatchesSearchPatter(momentItems[_searchIndex]));
            if (_searchIndex == -1)
            {
                AlertSearchReachedEnd();
                _isSearching = false;
            }
            else
            {
                ScrollToAndHighlight_FoundMoment(momentItems[_searchIndex]);
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
            ObservableCollection<MomentItemViewModel> momentItems = (BindingContext as MainPageViewModel)!.MomentItems!;
            if (_searchIndex == -1) { _searchIndex = _firstVisibleIndex - 1; }
            int searched = 0;
            do
            {
                _searchIndex++;
                if (_searchIndex > momentItems.Count - 1)
                {
                    Moment? previousMoment = await DatabaseService.Instance.GetPreviousMomentAsync();
                    if (previousMoment != null)
                    {
                        (BindingContext as MainPageViewModel)!.MomentItems!.Add(new MomentItemViewModel(previousMoment));
                        if (++searched % MauiProgram.SEARCH_PROMPT_LIMIT == 0)
                        {
                            MomentsCollectionView.ScrollTo(momentItems[_searchIndex], ScrollToPosition.MakeVisible);
                            bool result = await DisplayAlert("", AppResources.SearchLimitReachedText, AppResources.Yes, AppResources.No);
                            if (!result) { break; }
                        }
                    }
                    else
                    {
                        _searchIndex = -1;
                        break;
                    }
                }
            } while (!MomentItemMatchesSearchPatter(momentItems[_searchIndex]));
            if (_searchIndex == -1)
            {
                AlertSearchReachedEnd();
                _isSearching = false;
            }
            else
            {
                ScrollToAndHighlight_FoundMoment(momentItems[_searchIndex]);
            }
        }
        private async void AlertSearchReachedEnd()
        {
            await DisplayAlert("", AppResources.SearchMsgNoMoreResults, "Ok");
        }
        private async void ScrollToAndHighlight_FoundMoment(MomentItemViewModel momentItem)
        {
            MomentsCollectionView.ScrollTo(momentItem, ScrollToPosition.End);
            await Task.Delay(1000);
            MomentsCollectionView.SelectedItem = momentItem;
            await Task.Delay(200);
            MomentsCollectionView.SelectedItem = null;
            await Task.Delay(200);
            MomentsCollectionView.SelectedItem = momentItem;
            await Task.Delay(200);
            MomentsCollectionView.SelectedItem = null;
            await Task.Delay(200);
            MomentsCollectionView.SelectedItem = momentItem;
            await Task.Delay(200);
            MomentsCollectionView.SelectedItem = null;
            _isSearching = false;
        }
        private bool MomentItemMatchesSearchPatter(MomentItemViewModel momentItem)
        {
            string[] filters = SearchEntry.Text.Split(",");
            foreach (string filter in filters)
            {
                if (!string.IsNullOrEmpty(filter))
                {
                    if ((momentItem.Icon != null && 
                        momentItem.Icon.Contains(filter, StringComparison.OrdinalIgnoreCase)) ||
                        (momentItem.CreatedAtString != null && 
                        momentItem.CreatedAtString.Contains(filter, StringComparison.OrdinalIgnoreCase)) ||
                        (momentItem.Headline != null 
                        && momentItem.Headline.Contains(filter, StringComparison.OrdinalIgnoreCase)) ||
                        (momentItem.Description != null && 
                        momentItem.Description.Contains(filter, StringComparison.OrdinalIgnoreCase)))
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
