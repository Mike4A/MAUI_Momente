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

        public string Headline { get; set; } = "";

        public string Description { get; set; } = "";        
        
        public string ColorString { get; set; } = MauiProgram.MOMENT_DEFAULT_COLOR.ToHex();       
    }
}
