using SQLite;
using System.ComponentModel;

namespace Momente
{
    public class Moment : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Ignore]
        public string CreatedAtString { get => CreatedAt.ToString("dddd, dd. MMMM yyyy, HH:mm"); }

        public string Icon { get; set; } = "";

        public string Headline { get; set; } = "";

        public string Description { get; set; } = "";

        public string ColorString { get; set; } = MauiProgram.MOMENT_DEFAULT_COLOR.ToHex();
        [Ignore]
        public Color Color
        {
            get => Color.Parse(ColorString);
            set {
                if (ColorString != value.ToHex())
                {
                    ColorString = value.ToHex();
                    OnPropertyChanged(nameof(Color));
                }   
            }
        }

        [Ignore]
        public Color TestColor
        {
            get => Color.Parse(ColorString);
            set
            {
                if (ColorString != value.ToHex())
                {
                    ColorString = value.ToHex();
                    OnPropertyChanged(nameof(Color));
                }
            }
        }

        [Ignore]
        public Color GlowColor
        {
            get
            {
                Color c = Color.Parse(ColorString);
                return c.WithLuminosity(c.GetLuminosity() + MauiProgram.MOMENT_LUMINOSITY_GLOW);
            }
        }
        public Color ShadowColor
        {
            get
            {
                Color c = Color.Parse(ColorString);
                return c.WithLuminosity(c.GetLuminosity() + MauiProgram.MOMENT_LUMINOSITY_SHADOW);
            }
        }
        [Ignore]
        public string? HasDescriptonString { get => String.IsNullOrEmpty(Description) ? null : "📎"; }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        { 
            PropertyChanged?.Invoke(propertyName, new PropertyChangedEventArgs(propertyName));
        }
    }
}
