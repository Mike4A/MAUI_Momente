using Momente.Enums;

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

    public Color Color
    {
        get => (Color)GetValue(ColorProperty);
        set
        {
            if (Color == value) { return; }
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
        if (oldValue == newValue) { return; }
        ColorSliderView view = (ColorSliderView)bindable;
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
        view.HueGraphicsView.Saturation = (float)view.SaturationSlider.Value;
        view.HueGraphicsView.Luminosity = (float)view.LuminositySlider.Value;
        view.HueGraphicsView.Invalidate();
        view.SaturationGraphicsView.Hue = (float)view.HueSlider.Value;
        view.SaturationGraphicsView.Luminosity = (float)view.LuminositySlider.Value;
        view.SaturationGraphicsView.Invalidate();
        view.LuminosityGraphicsView.Saturation = (float)view.SaturationSlider.Value;
        view.LuminosityGraphicsView.Hue = (float)view.HueSlider.Value;
        view.LuminosityGraphicsView.Invalidate();
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