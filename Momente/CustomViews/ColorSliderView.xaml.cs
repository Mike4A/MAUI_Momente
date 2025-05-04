using Momente.Enums;
using Momente.Services;

namespace Momente.CustomViews;

public partial class ColorSliderView : ContentView
{
    public ColorSliderView()
    {
        InitializeComponent();
        HueGraphicsView.ColorChannel = ColorChannel.Hue;
        HueGraphicsView.Drawable = HueGraphicsView;
        SaturationGraphicsView.ColorChannel = ColorChannel.Saturation;
        SaturationGraphicsView.Drawable = SaturationGraphicsView;
        LuminosityGraphicsView.ColorChannel = ColorChannel.Luminosity;
        LuminosityGraphicsView.Drawable = LuminosityGraphicsView;
    }

    private bool _ignoreColorChanges = false;
    private bool _applyingColorToSliders = false;

    public Color Color
    {
        get => (Color)GetValue(ColorProperty);
        set
        {
            if (ColorService.IsColorSimilar(Color, value) || _applyingColorToSliders)
            { return; }
            SetValue(ColorProperty, value);
        }
    }

    public static readonly BindableProperty ColorProperty =
        BindableProperty.Create(
            nameof(Color),
            typeof(Color),
            typeof(GlowingBorderView),
            Colors.Magenta,
            BindingMode.TwoWay,
            propertyChanged: OnColorChanged);

    private static void OnColorChanged(BindableObject bindable, object oldValue, object newValue)
    {        
        if (ColorService.IsColorSimilar((Color)oldValue, (Color)newValue))
        { return; }
        ColorSliderView view = (ColorSliderView)bindable;
        view._applyingColorToSliders = true;
        if (!view._ignoreColorChanges)
        {
            if (view.Color.GetSaturation() != 0 && view.Color.GetLuminosity() != 0 && view.Color.GetLuminosity() != 1)
            {
                view.HueSlider.Value = view.Color.GetHue();
            }
            if (view.Color.GetLuminosity() != 0 && view.Color.GetLuminosity() != 1)
            {
                view.SaturationSlider.Value = view.Color.GetSaturation();
            }
            if (view.Color.GetSaturation() != 0)
            {
                view.LuminositySlider.Value = view.Color.GetLuminosity();
            }
        }
        view.HueGraphicsView.Saturation = view.SaturationSlider.Value;
        view.HueGraphicsView.Luminosity = view.LuminositySlider.Value;
        view.HueGraphicsView.Invalidate();
        view.SaturationGraphicsView.Hue = view.HueSlider.Value;
        view.SaturationGraphicsView.Luminosity = view.LuminositySlider.Value;
        view.SaturationGraphicsView.Invalidate();
        view.LuminosityGraphicsView.Saturation = view.SaturationSlider.Value;
        view.LuminosityGraphicsView.Hue = view.HueSlider.Value;
        view.LuminosityGraphicsView.Invalidate();
        view._applyingColorToSliders = false;   
    }

    private void HueSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        ApplySlidedColor();
    }

    private void SaturationSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        ApplySlidedColor();
    }

    private void LuminositySlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        ApplySlidedColor();
    }

    private void ApplySlidedColor()
    {
        _ignoreColorChanges = true;
        Color = Color.FromHsla(HueSlider.Value, SaturationSlider.Value, LuminositySlider.Value);
        _ignoreColorChanges = false;
    }
}