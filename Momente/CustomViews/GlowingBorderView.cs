using Momente.Services;
using System.Runtime.CompilerServices;

namespace Momente.CustomViews
{
    public class GlowingBorderView : GraphicsView, IDrawable
    {
        public GlowingBorderView()
        {
            Drawable = this;
        }

        public float BackgroundLuminosity
        {
            get => (float)GetValue(BackgroundLuminosityProperty);
            set => SetValue(BackgroundLuminosityProperty, value);
        }

        public static readonly BindableProperty BackgroundLuminosityProperty =
            BindableProperty.Create(
                nameof(BackgroundLuminosity),
                typeof(float),
                typeof(GlowingBorderView),
                0.5f,
                propertyChanged: OnBackgroundLuminosityChanged);

        static void OnBackgroundLuminosityChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as GlowingBorderView)!.Invalidate();
        }

        public Color GlowColor
        {
            get => (Color)GetValue(GlowColorProperty);
            set
            {
                if (ColorService.IsColorSimilar(GlowColor, value))
                { return; }
                SetValue(GlowColorProperty, value);
            }
        }

        public static readonly BindableProperty GlowColorProperty =
            BindableProperty.Create(
                nameof(GlowColor),
                typeof(Color),
                typeof(GlowingBorderView),
                 Color.FromRgb(127, 128, 128),
                propertyChanged: OnGlowColorChanged);

        static void OnGlowColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (ColorService.IsColorSimilar((Color)oldValue, (Color)newValue))
            { return; }
            (bindable as GlowingBorderView)!.Invalidate();
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(GlowingBorderView),
                new CornerRadius(0),
                propertyChanged: OnCornerRadiusChanged);

        static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as GlowingBorderView)!.Invalidate();
        }

        public float GlowOffset
        {
            get => (float)GetValue(GlowOffsetProperty);
            set => SetValue(GlowOffsetProperty, value);
        }

        public static readonly BindableProperty GlowOffsetProperty =
            BindableProperty.Create(
                nameof(GlowOffset),
                typeof(float),
                typeof(GlowingBorderView),
                0.1f,
                propertyChanged: OnGlowOffsetPropertyChanged);

        static void OnGlowOffsetPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as GlowingBorderView)!.Invalidate();
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float strokeSize = 1f;
            canvas.StrokeSize = strokeSize;
            canvas.FillColor = GlowColor.WithLuminosity(BackgroundLuminosity);
            canvas.FillRoundedRectangle(
               dirtyRect.X + strokeSize / 2 + 2,
               dirtyRect.Y + strokeSize / 2 + 2,
               dirtyRect.Width - strokeSize - 4,
               dirtyRect.Height - strokeSize - 4,
               (float)CornerRadius.TopLeft,
               (float)CornerRadius.TopRight,
               (float)CornerRadius.BottomLeft,
               (float)CornerRadius.BottomRight);

            strokeSize = 6f;
            canvas.StrokeSize = strokeSize;
            canvas.StrokeColor = GlowColor.WithLuminosity(GlowColor.GetLuminosity() - GlowOffset);
            canvas.DrawRoundedRectangle(
                dirtyRect.X + strokeSize / 2,
                dirtyRect.Y + strokeSize / 2,
                dirtyRect.Width - strokeSize,
                dirtyRect.Height - strokeSize,
                (float)CornerRadius.TopLeft,
                (float)CornerRadius.TopRight,
                (float)CornerRadius.BottomLeft,
                (float)CornerRadius.BottomRight);

            strokeSize = 4f;
            canvas.StrokeSize = strokeSize;
            canvas.StrokeColor = GlowColor;
            canvas.DrawRoundedRectangle(
                dirtyRect.X + strokeSize / 2 + 1,
                dirtyRect.Y + strokeSize / 2 + 1,
                dirtyRect.Width - strokeSize - 2,
                dirtyRect.Height - strokeSize - 2,
                (float)CornerRadius.TopLeft,
                (float)CornerRadius.TopRight,
                (float)CornerRadius.BottomLeft,
                (float)CornerRadius.BottomRight);

            strokeSize = 2f;
            canvas.StrokeSize = strokeSize;
            canvas.StrokeColor = GlowColor.WithLuminosity(GlowColor.GetLuminosity() + GlowOffset);
            canvas.DrawRoundedRectangle(
                dirtyRect.X + strokeSize / 2 + 2,
                dirtyRect.Y + strokeSize / 2 + 2,
                dirtyRect.Width - strokeSize - 4,
                dirtyRect.Height - strokeSize - 4,
                (float)CornerRadius.TopLeft,
                (float)CornerRadius.TopRight,
                (float)CornerRadius.BottomLeft,
                (float)CornerRadius.BottomRight);
        }
    }
}
