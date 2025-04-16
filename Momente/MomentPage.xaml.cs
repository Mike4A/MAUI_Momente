
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
            await Navigation.PopModalAsync();
        }
    }
}