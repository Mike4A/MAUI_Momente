
namespace Momente;

public partial class MomentPage : ContentPage
{    
    public MomentPage(int id, bool edit)
    {
        InitializeComponent();
        MomentViewModel viewModel = (BindingContext as MomentViewModel)!;
        TrySetViewModelId(viewModel, id);
        if (edit)
        { 
            //switch control visibility
        }
    }

    private async void TrySetViewModelId(MomentViewModel viewModel, int id)
    {
        bool result = await viewModel.TryLoadFromIdOrDefaultAsync(id);
        if (!result)
        {
            await DisplayAlert("", $"Es wurde kein Moment mit der Id {id} gefunden.", "Ok");
            await Navigation.PopAsync();
        }
    }

    private void IconLabel_Focused(object sender, FocusEventArgs e)
    {
        SelectIconLabelText();  
    }

    private void IconLabel_TextChanged(object sender, TextChangedEventArgs e)
    {
        SelectIconLabelText();
    }

    private void SelectIconLabelText()
    {
        Dispatcher.Dispatch(() =>
        {
            IconLabel.CursorPosition = 0;
            IconLabel.SelectionLength = IconLabel.Text.Length + 1;
        });
    }

    private void DeleteButton_Clicked(object sender, EventArgs e)
    {

    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {

    }
}