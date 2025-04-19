
using System.Globalization;

namespace Momente;

public partial class MomentPage : ContentPage
{
    public MomentPage(Moment moment)
    {
        InitializeComponent();
        _moment = moment;
        MomentViewModel viewModel = (BindingContext as MomentViewModel)!;        
        viewModel.Id = moment.Id;
        viewModel.CreatedAt = moment.CreatedAt;
        viewModel.CreatedAtString = moment.CreatedAt.ToString("dddd, dd. MMMM yyyy, HH:mm");
        viewModel.Icon = moment.Icon;
        viewModel.Headline = moment.Headline;
        viewModel.Description = moment.Description;       
    }

    private Moment _moment;

    private void IconEntry_Focused(object sender, FocusEventArgs e)
    {
        SelectIconLabelText();
    }
    private void IconEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        SelectIconLabelText();
    }
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
        AnimationService.AnimateButton(DeleteButton);
        if ((BindingContext as MomentViewModel)!.Id != 0)
        {
            var answer = await DisplayAlert("", "Moment löschen?", "Ja", "Nein");
            if (answer)
            {
                await DatabaseService.Instance.DeleteMomentAsync((BindingContext as MomentViewModel)!.Id);
            }
        }
        await Navigation.PopAsync();
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        AnimationService.AnimateButton(CancelButton);
        await Navigation.PopAsync();
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        AnimationService.AnimateButton(SaveButton);
        MomentViewModel viewModel = (BindingContext as MomentViewModel)!;
        _moment.Id = viewModel.Id;
        _moment.CreatedAt = viewModel.CreatedAt; 
        _moment.Icon = viewModel.Icon;
        _moment.Headline = viewModel.Headline;
        _moment.Description = viewModel.Description;
        if (await DatabaseService.Instance.GetMomentByIdAsync(_moment.Id) != null)
        {
            await DatabaseService.Instance.UpdateMomentAsync(_moment);
        }
        else
        {
            await DatabaseService.Instance.AddMomentAsync(_moment);
        }
        await Navigation.PopAsync();
    }
}