using Momente.Enums;
using Momente.Models;
using Momente.Resources.Localizations;
using Momente.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Momente.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public MainPageViewModel(MainPage mainPage)
        {
            _mainPage = mainPage;
            AddButtonCommand = new Command(async () => await AddButton_Clicked());
            MomentItemsSelectionChangedCommand = new Command(async () => await MomentItems_SelectionChanged());
            SearchMomentButtonCommand = new Command(SearchMomentButton_Clicked);
        }

        private MainPage _mainPage;

        private MomentPageArgs _momentPageArgs = new();

        private int _firstVisibleIndex = 0;

        private int _lastVisibleIndex = 0;

        private int _searchIndex;

        public ObservableCollection<MomentItemViewModel> MomentItems { get; } = [];

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _isSearching = false;

        public bool IsSearching
        {
            get => _isSearching;
            set
            {
                if (_isSearching == value) { return; }
                _isSearching = value;
                OnPropertyChanged(nameof(IsSearching));
            }
        }

        private MomentItemViewModel? _selectedMomentItem;

        public MomentItemViewModel? SelectedMomentItem
        {
            get => _selectedMomentItem;
            set
            {
                if (_selectedMomentItem == value) return;
                _selectedMomentItem = value;
                OnPropertyChanged(nameof(SelectedMomentItem));
            }
        }

        private string? _searchFilter;

        public string? SearchFilter
        {
            get => _searchFilter;
            set
            {
                if (_searchFilter == value) return;
                _searchFilter = value;
                OnPropertyChanged(nameof(SearchFilter));
            }
        }

        internal async void PopulateMomentsView()
        {
            //If an item is selected, the momentPage was navigated to and now we're back
            if (SelectedMomentItem != null)
            {
                if (_momentPageArgs.Action is MomentAction.Deleted)
                {
                    MomentItems.Remove(SelectedMomentItem);
                }
                if (_momentPageArgs.Action == MomentAction.Updated)
                {
                    MomentItemViewModel updatedMomentItem = new MomentItemViewModel(_momentPageArgs.Moment);
                    MomentItems[MomentItems.IndexOf(SelectedMomentItem)] = updatedMomentItem;
                    _mainPage.ScrollTo(updatedMomentItem);
                }
                _mainPage.ResetItemSelection();
            }
            else
            {
                if (MomentItems.Count == 0 || _momentPageArgs.Action == MomentAction.Created)
                {
                    Moment? lastMoment = await DatabaseService.Instance.GetLastMomentAsync();
                    if (lastMoment != null)
                    {
                        MomentItemViewModel lastMomentItem = new MomentItemViewModel(lastMoment);
                        MomentItems.Insert(0, lastMomentItem);
                        _mainPage.ScrollTo(lastMomentItem);
                    }
                }
            }
            _momentPageArgs = new MomentPageArgs();
        }

        internal async void SatisfyItemsThreshold()
        {
            Moment? previousMoment = await DatabaseService.Instance.GetPreviousMomentAsync();
            if (previousMoment != null)
            {
                MomentItems!.Add(new MomentItemViewModel(previousMoment));
            }
        }

        public ICommand AddButtonCommand { get; }

        private async Task AddButton_Clicked()
        {
            await Task.Delay(300);
            _mainPage.ResetItemSelection();
            await _mainPage.Navigation.PushModalAsync(new MomentPage(_momentPageArgs));
        }

        public ICommand MomentItemsSelectionChangedCommand { get; }

        private async Task MomentItems_SelectionChanged()
        {
            if (!IsSearching && SelectedMomentItem != null)
            {
                if (SelectedMomentItem.Headline == MauiProgram.DEV_CHEAT_CODE && SelectedMomentItem.Color!.ToHex() == MauiProgram.DEV_CHEAT_COlOR)
                {
                    string msg =
                        $"DB Path: {DatabaseService.Instance.NewDbPath}\n" +
                        $"DB Count: {await DatabaseService.Instance.GetCount()}\n" +
                        $"Loaded Moments: {MomentItems.Count}";
                    await _mainPage.DisplayAlert("", msg, "Ok");
                }
                _momentPageArgs = new MomentPageArgs((await DatabaseService.Instance.GetMomentByIdAsync(SelectedMomentItem.Id))!);
                await _mainPage.Navigation.PushModalAsync(new MomentPage(_momentPageArgs));
            }
        }

        internal void MomentItems_Scrolled(ItemsViewScrolledEventArgs e)
        {
            _firstVisibleIndex = e.FirstVisibleItemIndex;
            _lastVisibleIndex = e.LastVisibleItemIndex;
            if (!IsSearching)
            {
                _searchIndex = -1;
                if (MomentItems != null && MomentItems.Count > 0)
                {
                    //Drop invisible items
                    for (int i = MomentItems.Count - 1; i > _lastVisibleIndex + 3; i--)
                    {
                        MomentItems.Remove(MomentItems[i]);
                        DatabaseService.Instance.IdCounter = MomentItems[i - 1].Id;
                    }
                }
            }
        }
        public ICommand SearchMomentButtonCommand { get; }

        private void SearchMomentButton_Clicked(object obj)
        {
            _searchIndex = -1;
        }

        public async Task FindPreviousMomentItem()
        {

            if (IsSearching || String.IsNullOrEmpty(SearchFilter)) { return; }
            IsSearching = true;
            if (_searchIndex == -1) { _searchIndex = _firstVisibleIndex - 1; }
            int searched = 0;
            do
            {
                _searchIndex++;
                if (_searchIndex > MomentItems.Count - 1)
                {
                    Moment? previousMoment = await DatabaseService.Instance.GetPreviousMomentAsync();
                    if (previousMoment != null)
                    {
                        MomentItems.Add(new MomentItemViewModel(previousMoment));
                        if (++searched % MauiProgram.SEARCH_PROMPT_LIMIT == 0)
                        {
                            _mainPage.ScrollTo(MomentItems[_searchIndex], animate: false);

                            if (!await _mainPage.DisplayAlert("", AppResources.SearchLimitReachedText, AppResources.Yes, AppResources.No))
                            { break; }
                        }
                    }
                    else
                    {
                        _searchIndex = -1;
                        break;
                    }
                }
            } while (!MomentItemMatchesSearchPatter(MomentItems[_searchIndex]));
            if (_searchIndex == -1)
            {
                await _mainPage.AlertSearchReachedEnd();
                IsSearching = false;
            }
            else
            {
                _mainPage.ScrollToAndHighlight_FoundMoment(MomentItems[_searchIndex]);
            }
        }

        public async Task FindNextMomentItem()
        {
            if (IsSearching || String.IsNullOrEmpty(SearchFilter)) { return; }
            IsSearching = true;
            if (_searchIndex == -1) { _searchIndex = _lastVisibleIndex + 1; }
            int searched = 0;
            do
            {
                _searchIndex--;
                if (++searched % MauiProgram.SEARCH_PROMPT_LIMIT == 0)
                {
                    if (_searchIndex > -1)
                    {
                        _mainPage.ScrollTo(MomentItems[_searchIndex], animate: false);
                    }

                    if (!await _mainPage.DisplayAlert("", AppResources.SearchLimitReachedText, AppResources.Yes, AppResources.No))
                    { break; }
                }

            } while (_searchIndex > -1 && !MomentItemMatchesSearchPatter(MomentItems[_searchIndex]));
            if (_searchIndex == -1)
            {
                await _mainPage.AlertSearchReachedEnd();
                IsSearching = false;
            }
            else
            {
                _mainPage.ScrollToAndHighlight_FoundMoment(MomentItems[_searchIndex]);
            }
        }

        public bool MomentItemMatchesSearchPatter(MomentItemViewModel momentItem)
        {
            if (string.IsNullOrEmpty(SearchFilter)) { return false; }
            string[] filters = SearchFilter.Split(",");
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
    }
}
