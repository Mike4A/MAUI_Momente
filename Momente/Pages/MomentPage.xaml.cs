using Momente.Drawables;
using Momente.Models;
using Momente.ViewModels;

namespace Momente;

public partial class MomentPage : ContentPage
{
    public MomentPage(MomentPageArgs args)
    {
        InitializeComponent();
        _args = args;
        BindingContext = _viewModel = new MomentPageViewModel(this, args);
        float hue = Color.Parse(args.Moment.ColorString).GetHue();
        float saturation = Color.Parse(args.Moment.ColorString).GetSaturation();
        float luminosity = Color.Parse(args.Moment.ColorString).GetLuminosity();
        HueGraphicsView.Drawable = _hueDrawable = new HueDrawable(saturation, luminosity);
        SaturationGraphicsView.Drawable = _saturationDrawable = new SaturationDrawable(hue, luminosity);
        LuminosityGraphicsView.Drawable = _luminosityDrawable = new LuminosityDrawable(hue, saturation);
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