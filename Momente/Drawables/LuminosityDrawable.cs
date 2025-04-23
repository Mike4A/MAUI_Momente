using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Momente.Drawables
{
    internal class LuminosityDrawable : IDrawable
    {
        public LuminosityDrawable(float hue, float saturation)
        {
            Hue = hue;
            Saturation = saturation;
        }
                
        public float Hue { get; set; }
        public float Saturation { get; set; }
        
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            int strokeSize = 3;
            canvas.StrokeSize = strokeSize;
            for (int x = -12; x < dirtyRect.Width + 12; x += strokeSize)
            {
                float Luminosity = (x + 1) / dirtyRect.Width;
                canvas.StrokeColor = Color.FromHsla(Hue, Saturation, Luminosity);
                canvas.DrawLine(x, 0, x, dirtyRect.Height - 1);
            }
        }
    }
}
