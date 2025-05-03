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

    private bool _isApplyingColorToSliders = false;

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
        ColorSliderView view = (ColorSliderView)bindable;
        view._isApplyingColorToSliders = true;
        view.HueSlider.Value = view.Color.GetHue();
        view.SaturationSlider.Value = view.Color.GetSaturation();
        view.LuminositySlider.Value = view.Color.GetLuminosity();
        view._isApplyingColorToSliders = false;
    }

    private void HueSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        ApplySliderChanges();
    }

    private void SaturationSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        ApplySliderChanges();
    }

    private void LuminositySlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        ApplySliderChanges();
    }

    private void ApplySliderChanges()
    {
        if (_isApplyingColorToSliders) { return; }
        Color = Color.FromHsla(HueSlider.Value, SaturationSlider.Value, LuminositySlider.Value, 1);
    }
}