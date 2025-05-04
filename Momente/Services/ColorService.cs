using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Momente.Services
{
    internal static class ColorService
    {
        internal static bool IsColorSimilar(Color color1, Color color2, float tolerance = 0.001f)
        {
            return Math.Abs(color1.Red - color2.Red) < tolerance &&
                   Math.Abs(color1.Green - color2.Green) < tolerance &&
                   Math.Abs(color1.Blue - color2.Blue) < tolerance &&
                   Math.Abs(color1.Alpha - color2.Alpha) < tolerance;
        }
    }
}
