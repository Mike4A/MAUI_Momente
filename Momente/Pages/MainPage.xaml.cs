using Momente.Resources.Localizations;
using Momente.Services;
using Momente.ViewModels;

namespace Momente
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new MainPageViewModel(this);
            SearchEntry.Placeholder = AppResources.SearchfilterPlaceholder;
        }

        private readonly MainPageViewModel _viewModel;

        private void MainPage_NavigatedTo(object? sender, NavigatedToEventArgs e)
        {
            _viewModel.PopulateMomentsView();
        }

        private void MomentsCollectionView_RemainingItemsThresholdReached(object sender, EventArgs e)
        {
            _viewModel.SatisfyItemsThreshold();
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
            _ = DebugService.WriteMomentEntries();
#endif
        }

        private async void SearchMomentsButton_Clicked(object sender, EventArgs e)
        {
            await SearchMomentsButton.ScaleTo(0.75, 50);
            await SearchMomentsButton.RotateXTo(180, 100);
            await SearchMomentsButton.RotateXTo(0, 100);
            await SearchMomentsButton.ScaleTo(1, 50);
            DefaultControlsGrid.IsVisible = false;
            SearchControlsGrid.IsVisible = true;
            SearchEntry.Focus();
        }

        private async void AddMomentButton_Clicked(object sender, EventArgs e)
        {
            await AddMomentButton.ScaleTo(0.75, 50);
            await AddMomentButton.RotateXTo(180, 100);
            await AddMomentButton.RotateXTo(0, 100);
            await AddMomentButton.ScaleTo(1, 50);
        }

        private void MomentsCollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            _viewModel.MomentItems_Scrolled(e);
        }

        private async void FindNextButton_Clicked(object sender, EventArgs e)
        {
            await FindNextButton.ScaleTo(0.75, 50);
            await FindNextButton.RotateXTo(180, 100);
            await FindNextButton.RotateXTo(0, 100);
            await FindNextButton.ScaleTo(1, 50);
            await _viewModel.FindNextMomentItem();
        }

        private async void FindPreviousButton_Clicked(object sender, EventArgs e)
        {
            await FindPreviousButton.ScaleTo(0.75, 50);
            await FindPreviousButton.RotateXTo(180, 100);
            await FindPreviousButton.RotateXTo(0, 100);
            await FindPreviousButton.ScaleTo(1, 50);
            await _viewModel.FindPreviousMomentItem();
        }

        public async Task AlertSearchReachedEnd()
        {
            await DisplayAlert("", AppResources.SearchMsgNoMoreResults, "Ok");
        }

        public async void ScrollToAndHighlight_FoundMoment(MomentItemViewModel momentItem)
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
            _viewModel.IsSearching = false;
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

        internal void ScrollTo(MomentItemViewModel updatedMomentItem, ScrollToPosition scrollToPosition = ScrollToPosition.MakeVisible)
        {
            MomentsCollectionView.ScrollTo(updatedMomentItem, scrollToPosition);
        }

        internal void ResetItemSelection()
        {
            MomentsCollectionView.SelectedItem = null;
        }
    }
}
