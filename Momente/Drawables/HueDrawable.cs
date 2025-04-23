using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Momente.Drawables
{
    internal class HueDrawable : IDrawable
    {
        public HueDrawable(float saturation, float luminosity)
        {
            Saturation = saturation;
            Luminosity = luminosity;
        }

        public float Saturation { get; set; }
        public float Luminosity { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeSize = 1;
            for (int x = -12; x < dirtyRect.Width + 12; x++)
            {
                float hue = (x + 1) / dirtyRect.Width;
                canvas.StrokeColor = Color.FromHsla(hue, Saturation, Luminosity);
                canvas.DrawLine(x, 0, x, dirtyRect.Height - 1);
            }
        }
    }
}
