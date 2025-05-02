using SQLite;
using System.ComponentModel;

namespace Momente.Models
{
    public class Moment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Icon { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Ignore]
        public string CreatedAtString { get => CreatedAt.ToString("dddd, dd. MMMM yyyy, HH:mm"); }

        public string Headline { get; set; } = "";

        public string Description { get; set; } = "";

        [Ignore]
        public string? HasDescriptonIcon { get => string.IsNullOrEmpty(Description) ? null : "📎"; }
        
        public string ColorString { get; set; } = MauiProgram.MOMENT_DEFAULT_COLOR.ToHex();

        [Ignore]
        public Color Color
        {
            get => Color.Parse(ColorString);
            set
            {
                if (ColorString != value.ToHex())
                {
                    ColorString = value.ToHex();
                }
            }
        }
    }
}
