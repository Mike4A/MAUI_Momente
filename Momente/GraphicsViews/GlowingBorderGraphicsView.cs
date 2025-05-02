using System.Runtime.CompilerServices;

namespace Momente.GraphicsViews
{
    public class GlowingBorderGraphicsView : GraphicsView, IDrawable
    {
        public GlowingBorderGraphicsView()
        {
            Drawable = this;
        }

        private static Color DEFAULT_BACKGROUND_COLOR = Colors.Black;
        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }
        public static readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create(
                nameof(BackgroundColor),
                typeof(Color),
                typeof(GlowingBorderGraphicsView),
                DEFAULT_BACKGROUND_COLOR,
                propertyChanged: OnBackgroundColorChanged);
        static void OnBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as GlowingBorderGraphicsView)!.Invalidate();
        }

        private static Color DEFAULT_GLOW_COLOR = Color.FromRgb(127, 128, 128);
        public Color GlowColor
        {
            get => (Color)GetValue(GlowColorProperty);
            set => SetValue(GlowColorProperty, value);
        }
        public static readonly BindableProperty GlowColorProperty =
            BindableProperty.Create(
                nameof(GlowColor),
                typeof(Color),
                typeof(GlowingBorderGraphicsView),
                DEFAULT_GLOW_COLOR,
                propertyChanged: OnGlowColorChanged);
        static void OnGlowColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as GlowingBorderGraphicsView)!.Invalidate();
        }

        private const string DEFAULT_CORNER_RADIUS = "0";
        public string CornerRadius
        {
            get => (string)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(
                nameof(CornerRadius),
                typeof(string),
                typeof(GlowingBorderGraphicsView),
                DEFAULT_CORNER_RADIUS,
                propertyChanged: OnCornerRadiusChanged);
        static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as GlowingBorderGraphicsView)!.Invalidate();
        }
        private string[] CornerRadii
        {
            get
            {
                string[] cornerRadii = CornerRadius.Split(',');
                if (cornerRadii.Length == 1)
                {
                    cornerRadii = new string[] { cornerRadii[0], cornerRadii[0], cornerRadii[0], cornerRadii[0] };
                }
                return cornerRadii;
            }
        }

        private const float DEFAULT_GLOW_OFFSET = 0.1f;
        public float GlowOffset
        {
            get => (float)GetValue(GlowOffsetProperty);
            set => SetValue(GlowOffsetProperty, value);
        }
        public static readonly BindableProperty GlowOffsetProperty =
            BindableProperty.Create(
                nameof(CornerRadius),
                typeof(float),
                typeof(GlowingBorderGraphicsView),
                DEFAULT_GLOW_OFFSET,
                propertyChanged: OnGlowOffsetPropertyChanged);
        static void OnGlowOffsetPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as GlowingBorderGraphicsView)!.Invalidate();
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float strokeSize = 1f;
            canvas.StrokeSize = strokeSize;
            canvas.FillColor = BackgroundColor;
            canvas.FillRoundedRectangle(
               dirtyRect.X + strokeSize / 2 + 2,
               dirtyRect.Y + strokeSize / 2 + 2,
               dirtyRect.Width - strokeSize - 4,
               dirtyRect.Height - strokeSize - 4,
               float.Parse(CornerRadii[0]),
               float.Parse(CornerRadii[1]),
               float.Parse(CornerRadii[2]),
               float.Parse(CornerRadii[3]));

            strokeSize = 6f;
            canvas.StrokeSize = strokeSize;
            canvas.StrokeColor = GlowColor.WithLuminosity(GlowColor.GetLuminosity() - GlowOffset);
            canvas.DrawRoundedRectangle(
                dirtyRect.X + strokeSize / 2,
                dirtyRect.Y + strokeSize / 2,
                dirtyRect.Width - strokeSize,
                dirtyRect.Height - strokeSize,
                float.Parse(CornerRadii[0]),
                float.Parse(CornerRadii[1]),
                float.Parse(CornerRadii[2]),
                float.Parse(CornerRadii[3]));

            strokeSize = 4f;
            canvas.StrokeSize = strokeSize;
            canvas.StrokeColor = GlowColor;
            canvas.DrawRoundedRectangle(
                dirtyRect.X + strokeSize / 2 + 1,
                dirtyRect.Y + strokeSize / 2 + 1,
                dirtyRect.Width - strokeSize - 2,
                dirtyRect.Height - strokeSize - 2,
                float.Parse(CornerRadii[0]),
                float.Parse(CornerRadii[1]),
                float.Parse(CornerRadii[2]),
                float.Parse(CornerRadii[3]));

            strokeSize = 2f;
            canvas.StrokeSize = strokeSize;
            canvas.StrokeColor = GlowColor.WithLuminosity(GlowColor.GetLuminosity() + GlowOffset);
            canvas.DrawRoundedRectangle(
                dirtyRect.X + strokeSize / 2 + 2,
                dirtyRect.Y + strokeSize / 2 + 2,
                dirtyRect.Width - strokeSize - 4,
                dirtyRect.Height - strokeSize - 4,
                float.Parse(CornerRadii[0]),
                float.Parse(CornerRadii[1]),
                float.Parse(CornerRadii[2]),
                float.Parse(CornerRadii[3]));
        }
    }
}
