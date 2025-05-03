using Momente.Models;
using Momente.Resources.Localizations;
using Momente.ViewModels;

namespace Momente;

public partial class MomentPage : ContentPage
{
    public MomentPage(MomentPageArgs args)
    {
        InitializeComponent();
        _args = args;
        BindingContext = _viewModel = new MomentPageViewModel(this, args);
        HeadlineEntry.Placeholder = AppResources.HeadlinePlaceholder;
        DescriptionEditor.Placeholder = AppResources.DescriptionPlaceholder;
    }

    private readonly MomentPageViewModel _viewModel;

    public MomentPageArgs _args;

    private void IconEntry_Focused(object sender, FocusEventArgs e) { SelectIconLabelText(); }

    private void IconEntry_TextChanged(object sender, TextChangedEventArgs e) { SelectIconLabelText(); }

    private void SelectIconLabelText()
    {
        Dispatcher.Dispatch(() =>
          {
              if (!String.IsNullOrEmpty(IconEntry.Text))
              {
                  IconEntry.CursorPosition = 0;
                  IconEntry.SelectionLength = IconEntry.Text.Length;
              }
          });
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        await DeleteButton.ScaleTo(0.75, 50);
        await DeleteButton.RotateXTo(180, 100);
        await DeleteButton.RotateXTo(0, 100);
        await DeleteButton.ScaleTo(1, 50);
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await CancelButton.ScaleTo(0.75, 50);
        await CancelButton.RotateXTo(180, 100);
        await CancelButton.RotateXTo(0, 100);
        await CancelButton.ScaleTo(1, 50);
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        await SaveButton.ScaleTo(0.75, 50);
        await SaveButton.RotateXTo(180, 100);
        await SaveButton.RotateXTo(0, 100);
        await SaveButton.ScaleTo(1, 50);
    }
    private void IconEntry_Completed(object sender, EventArgs e)
    {
        IconEntry.Unfocus();
    }
    private void HeadlineEntry_Completed(object sender, EventArgs e)
    {
        HeadlineEntry.Unfocus();
    }
    private void DescriptionEditor_Completed(object sender, EventArgs e)
    {
        DescriptionEditor.Unfocus();
    }
}