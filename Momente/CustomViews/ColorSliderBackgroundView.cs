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
        public ColorSliderBackgroundView()
        {
            Drawable = this;
            InputTransparent = true;
        }
        public ColorChannel ColorChannel { get; set; }

        public double Hue { get; set; }

        public double Saturation { get; set; }

        public double Luminosity { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float strokeSize = dirtyRect.Width / 100;
            canvas.StrokeSize = strokeSize;
            canvas.Antialias = false;
            canvas.BlendMode = BlendMode.Overlay;
            for (double x = -12; x < dirtyRect.Width + 12; x += strokeSize)
            {
                double channelValue = (x + 1) / dirtyRect.Width;
                switch (ColorChannel)
                {
                    case ColorChannel.Hue:
                        canvas.StrokeColor = Color.FromHsla(channelValue, Saturation, Luminosity);
                        break;
                    case ColorChannel.Saturation:
                        canvas.StrokeColor = Color.FromHsla(Hue, channelValue, Luminosity);
                        break;
                    case ColorChannel.Luminosity:
                        canvas.StrokeColor = Color.FromHsla(Hue, Saturation, channelValue);
                        break;
                    case ColorChannel.None:
                    default:
                        break;
                }
                canvas.DrawLine((float)x, 0, (float)x, dirtyRect.Height - 1);
            }
        }
    }
}
