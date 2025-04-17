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
        public string Icon { get; set; } = "🙂";
        public string Headline { get; set; } = "";
        public string Description { get; set; } = "";

        public Moment() { }
    }
}
