
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
        viewModel.Color = moment.Color;
        HueSwitch.IsToggled = moment.Color.ToHex() != "#808080";
        HueSlider.Value = moment.Color.GetHue();
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
            if (await DisplayAlert("", "Moment löschen?", "Ja", "Nein"))
            {
                await DatabaseService.Instance.DeleteMomentAsync((BindingContext as MomentViewModel)!.Id);
                await Navigation.PopModalAsync();
            }
        }
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        AnimationService.AnimateButton(CancelButton);
        await Navigation.PopModalAsync();
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        AnimationService.AnimateButton(SaveButton);
        await SaveChanges();
    }

    private async Task SaveChanges()
    {
        MomentViewModel viewModel = (BindingContext as MomentViewModel)!;
        _moment.Id = viewModel.Id;
        _moment.CreatedAt = viewModel.CreatedAt;
        _moment.Icon = viewModel.Icon;
        _moment.Headline = viewModel.Headline;
        _moment.Description = viewModel.Description;
        _moment.Color = viewModel.Color;
        if (await DatabaseService.Instance.GetMomentByIdAsync(_moment.Id) != null)
        {
            await DatabaseService.Instance.UpdateMomentAsync(_moment);
        }
        else
        {
            await DatabaseService.Instance.AddMomentAsync(_moment);
        }
        await Navigation.PopModalAsync();
    }

    private void HueSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        HueSlider.IsEnabled = HueSwitch.IsToggled;
        if (HueSlider.IsEnabled)
        {
            (BindingContext as MomentViewModel)!.Color = Colors.White.WithHue((float)HueSlider.Value).WithLuminosity(0.74f).WithSaturation(1);
        }
        else
        {
            (BindingContext as MomentViewModel)!.Color = Color.Parse("#808080");
        }        
    }

    private void HueSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        Console.WriteLine($"{(float)HueSlider.Value}");
        (BindingContext as MomentViewModel)!.Color = Color.FromHsv((float)HueSlider.Value, 1, 0.5f);
    }
}