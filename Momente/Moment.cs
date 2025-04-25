using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Momente
{
    public class Moment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Ignore]
        public string CreatedAtString { get => CreatedAt.ToString("dddd, dd. MMMM yyyy, HH:mm"); }

        public string Icon { get; set; } = "";

        public string Headline { get; set; } = "";

        public string Description { get; set; } = "";

        public string ColorString { get; set; } = MauiProgram.DEFAULT_MOMENT_COLOR.ToHex();
        [Ignore]
        public Color Color { get => Color.Parse(ColorString); set => ColorString = value.ToHex(); }
        [Ignore]
        public Color GlowColor
        {
            get
            {
                Color c = Color.Parse(ColorString);
                return c.WithLuminosity(c.GetLuminosity() + MauiProgram.LUMINOSITY_GLOW);
            }
        }
        [Ignore]
        public string? HasDescriptonString { get => String.IsNullOrEmpty(Description) ? null : "📎"; }
    }
}
