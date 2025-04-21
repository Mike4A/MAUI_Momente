
using System.Globalization;

namespace Momente;

public partial class MomentPage : ContentPage
{
    public MomentPage(MomentPageArgs args)
    {
        InitializeComponent();
        MomentViewModel viewModel = (BindingContext as MomentViewModel)!;
        _args = args;        
        viewModel.Id = _args.Moment.Id;
        viewModel.CreatedAt = _args.Moment.CreatedAt;
        viewModel.CreatedAtString = _args.Moment.CreatedAt.ToString("dddd, dd. MMMM yyyy, HH:mm");
        viewModel.Icon = _args.Moment.Icon;
        viewModel.Headline = _args.Moment.Headline;
        viewModel.Description = _args.Moment.Description;
        viewModel.Color = _args.Moment.Color;
        HueSwitch.IsToggled = _args.Moment.Color.ToHex() != "#808080";
        HueSlider.Value = _args.Moment.Color.GetHue();
    }

    private MomentPageArgs _args;

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
        await DeleteButton.ScaleTo(0.75, 50);
        await DeleteButton.RotateXTo(180, 100);
        await DeleteButton.RotateXTo(0, 100);
        await DeleteButton.ScaleTo(1, 50);
        if ((BindingContext as MomentViewModel)!.Id != 0)
        {
            if (await DisplayAlert("", "Moment löschen?", "Ja", "Nein"))
            {
                await DatabaseService.Instance.DeleteMomentAsync((BindingContext as MomentViewModel)!.Id);
                _args.Action = MomentAction.Deleted;
                await Navigation.PopModalAsync();
            }
        }
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await CancelButton.ScaleTo(0.75, 50);
        await CancelButton.RotateXTo(180, 100);
        await CancelButton.RotateXTo(0, 100);
        await CancelButton.ScaleTo(1, 50);
        _args.Action = MomentAction.None;
        await Navigation.PopModalAsync();
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        await SaveButton.ScaleTo(0.75, 50);
        await SaveButton.RotateXTo(180, 100);
        await SaveButton.RotateXTo(0, 100);
        await SaveButton.ScaleTo(1, 50); 
        await SaveChanges();
        _args.Action = MomentAction.Saved;
        await Navigation.PopModalAsync();
    }

    private async Task SaveChanges()
    {
        MomentViewModel viewModel = (BindingContext as MomentViewModel)!;
        _args.Moment.Id = viewModel.Id;
        _args.Moment.CreatedAt = viewModel.CreatedAt;
        _args.Moment.Icon = viewModel.Icon;
        _args.Moment.Headline = viewModel.Headline;
        _args.Moment.Description = viewModel.Description;
        _args.Moment.Color = viewModel.Color;
        if (await DatabaseService.Instance.GetMomentByIdAsync(_args.Moment.Id) != null)
        {
            await DatabaseService.Instance.UpdateMomentAsync(_args.Moment);
        }
        else
        {
            await DatabaseService.Instance.AddMomentAsync(_args.Moment);
        }                
    }

    private void HueSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        HueSlider.IsEnabled = HueSwitch.IsToggled;
        if (HueSlider.IsEnabled)
        {
            (BindingContext as MomentViewModel)!.Color = Color.FromHsv((float)HueSlider.Value, 1, MauiProgram.MOMENT_COLOR_VALUE);
        }
        else
        {
            (BindingContext as MomentViewModel)!.Color = Color.Parse("#808080");
        }
    }

    private void HueSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        Console.WriteLine($"{(float)HueSlider.Value}");
        (BindingContext as MomentViewModel)!.Color = Color.FromHsv((float)HueSlider.Value, 1, MauiProgram.MOMENT_COLOR_VALUE);
    }
}