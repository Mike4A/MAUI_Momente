
namespace Momente;

public partial class MomentPage : ContentPage
{    
    public MomentPage(int id)
    {
        InitializeComponent();
        MomentViewModel viewModel = (BindingContext as MomentViewModel)!;
        TrySetViewModelId(viewModel, id);              
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
            if (!String.IsNullOrEmpty( IconLabel.Text))
            {
                IconLabel.CursorPosition = 0;
                IconLabel.SelectionLength = IconLabel.Text.Length;
            }
        });
    }

    private void DeleteButton_Clicked(object sender, EventArgs e)
    {

    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {

    }
}