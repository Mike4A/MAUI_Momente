using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Momente
{
    internal static class AnimationService
    {
        internal static async void AnimateButton(Button button)
        {
            await button.ScaleTo(0.75, 100);
            await button.RotateXTo(180, 100);
            await button.RotateXTo(0, 100);
            await button.ScaleTo(1, 100);            
        }
    }
}
