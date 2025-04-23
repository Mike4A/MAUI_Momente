using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Momente.Drawables
{
    internal class SaturationDrawable : IDrawable
    {
        public SaturationDrawable(float hue, float luminosity)
        {
            Hue = hue;
            Luminosity = luminosity;
        }

        public float Hue { get; set; }
        public float Luminosity { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeSize = 1;
            for (int x = -12; x < dirtyRect.Width + 12; x++)
            {
                float saturation = (x + 1) / dirtyRect.Width;
                canvas.StrokeColor = Color.FromHsla(Hue, saturation, Luminosity);
                canvas.DrawLine(x, 0, x, dirtyRect.Height - 1);
            }
        }
    }
}
