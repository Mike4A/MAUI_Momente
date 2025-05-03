using Momente.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Momente.CustomViews
{
    internal class ColorSliderBackgroundView : GraphicsView, IDrawable
    {
        public ColorChannel ColorChannel { get; set; }

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set { 
                if (Color == value) { return; }
                SetValue(ColorProperty, value); }
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
            ((ColorSliderBackgroundView)bindable).Invalidate();    
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float strokeSize = dirtyRect.Width / 100;
            canvas.StrokeSize = strokeSize;
            canvas.Antialias = false;
            canvas.BlendMode = BlendMode.Overlay;
            for (float x = -12; x < dirtyRect.Width + 12; x += strokeSize)
            {
                float channelValue = (x + 1) / dirtyRect.Width;
                switch (ColorChannel)
                {
                    case ColorChannel.Hue:
                        canvas.StrokeColor = Color.FromHsla(channelValue, Color.GetSaturation(), Color.GetLuminosity());
                        break;
                    case ColorChannel.Saturation:
                        canvas.StrokeColor = Color.FromHsla(Color.GetHue(), channelValue, Color.GetLuminosity());
                        break;
                    case ColorChannel.Luminosity:
                        canvas.StrokeColor = Color.FromHsla(Color.GetHue(), Color.GetSaturation(), channelValue);
                        break;
                    case ColorChannel.None:
                    default:
                        break;
                }
                canvas.DrawLine(x, 0, x, dirtyRect.Height - 1);
            }
        }
    }
}
