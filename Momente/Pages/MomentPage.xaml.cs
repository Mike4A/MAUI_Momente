
using Momente.Drawables;
using Momente.Enums;
using Momente.Models;
using Momente.Resources.Localizations;
using Momente.Services;
using Momente.ViewModels;

namespace Momente;

public partial class MomentPage : ContentPage
{
    public MomentPage(MomentPageArgs args)
    {
        InitializeComponent();
        _args = args;
        BindingContext = _viewModel = new MomentPageViewModel();
        float hue = args.Moment.Color.GetHue();
        float saturation = args.Moment.Color.GetSaturation();
        float luminosity = args.Moment.Color.GetLuminosity();
        HueGraphicsView.Drawable = _hueDrawable = new HueDrawable(saturation, luminosity);
        SaturationGraphicsView.Drawable = _saturationDrawable = new SaturationDrawable(hue, luminosity);
        LuminosityGraphicsView.Drawable = _luminosityDrawable = new LuminosityDrawable(hue, saturation);
        _viewModel.Id = args.Moment.Id;
        _viewModel.CreatedAt = args.Moment.CreatedAt;
        _viewModel.CreatedAtString = args.Moment.CreatedAt.ToString("dddd, dd. MMMM yyyy, HH:mm");
        _viewModel.Icon = args.Moment.Icon;
        _viewModel.Headline = args.Moment.Headline;
        _viewModel.Description = args.Moment.Description;
        _viewModel.Color = SlidedColor = args.Moment.Color;
    }

    private MomentPageViewModel _viewModel;

    private MomentPageArgs _args;

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
        if (_viewModel.Id != 0)
        {
            if (await DisplayAlert("", AppResources.DeleteMomentQuestion, AppResources.Yes, AppResources.No))
            {
                await DatabaseService.Instance.DeleteMomentAsync(_viewModel.Id);
                _args.Action = MomentAction.Deleted;
                await Navigation.PopAsync();
            }
        }
        else
        {
            _args.Action = MomentAction.None;
            await Navigation.PopAsync();
        }
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await CancelButton.ScaleTo(0.75, 50);
        await CancelButton.RotateXTo(180, 100);
        await CancelButton.RotateXTo(0, 100);
        await CancelButton.ScaleTo(1, 50);
        _args.Action = MomentAction.None;
        //Already tried to do this on NavigatedFrom event too, but it fails due to not being able to cancel the navigation and DB sync props
        if (ChangesMadeToMoment() && await DisplayAlert("", AppResources.SaveMomentQuestion, AppResources.Yes, AppResources.No))
        {
            SaveChangesAndPop();
        }
        else
        {
            await Navigation.PopAsync();
        }
    }

    private bool ChangesMadeToMoment()
    {
        if (_args.Moment.Icon != _viewModel.Icon)
        { return true; }
        if (_args.Moment.Headline != _viewModel.Headline)
        { return true; }
        if (_args.Moment.Description != _viewModel.Description)
        { return true; }
        if (_args.Moment.Color.ToHex() != _viewModel.Color.ToHex())
        { return true; }
        return false;
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        await SaveButton.ScaleTo(0.75, 50);
        await SaveButton.RotateXTo(180, 100);
        await SaveButton.RotateXTo(0, 100);
        await SaveButton.ScaleTo(1, 50);
        SaveChangesAndPop();
    }
    private async void SaveChangesAndPop()
    {
        _args.Moment.Id = _viewModel.Id;
        _args.Moment.CreatedAt = _viewModel.CreatedAt;
        _args.Moment.Icon = _viewModel.Icon;
        _args.Moment.Headline = _viewModel.Headline;
        _args.Moment.Description = _viewModel.Description;
        _args.Moment.Color = _viewModel.Color;
        if (await DatabaseService.Instance.GetMomentByIdAsync(_args.Moment.Id) != null)
        {
            await DatabaseService.Instance.UpdateMomentAsync(_args.Moment);
            _args.Action = MomentAction.Updated;
        }
        else
        {
            await DatabaseService.Instance.AddMomentAsync(_args.Moment);
            _args.Action = MomentAction.Created;
        }
        await Navigation.PopAsync();
    }

    public Color SlidedColor
    {
        get => Color.FromHsla(HueSlider.Value, SaturationSlider.Value, LuminositySlider.Value);
        set
        {
            Color newColor = value;
            if ((float)HueSlider.Value != newColor.GetHue())
                HueSlider.Value = newColor.GetHue();
            if ((float)SaturationSlider.Value != newColor.GetSaturation())
                SaturationSlider.Value = newColor.GetSaturation();
            if ((float)LuminositySlider.Value != newColor.GetLuminosity())
                LuminositySlider.Value = newColor.GetLuminosity();
        }
    }
    private HueDrawable _hueDrawable;
    private SaturationDrawable _saturationDrawable;
    private LuminosityDrawable _luminosityDrawable;
    private void HueSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        _saturationDrawable.Hue = (float)HueSlider.Value;
        SaturationGraphicsView.Invalidate();
        _luminosityDrawable.Hue = (float)HueSlider.Value;
        LuminosityGraphicsView.Invalidate();
        UpdateViewModelColor();
    }
    private void SaturationSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        var x = SlidedColor;
        _hueDrawable.Saturation = (float)SaturationSlider.Value;
        HueGraphicsView.Invalidate();
        _luminosityDrawable.Saturation = (float)SaturationSlider.Value;
        LuminosityGraphicsView.Invalidate();
        UpdateViewModelColor();
    }
    private void LuminositySlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        _hueDrawable.Luminosity = (float)LuminositySlider.Value;
        HueGraphicsView.Invalidate();
        _saturationDrawable.Luminosity = (float)LuminositySlider.Value;
        SaturationGraphicsView.Invalidate();
        UpdateViewModelColor();
    }
    private void UpdateViewModelColor()
    {
        if ((BindingContext as MomentPageViewModel)!.Color != SlidedColor)
        {
            (BindingContext as MomentPageViewModel)!.Color = SlidedColor;
        }
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