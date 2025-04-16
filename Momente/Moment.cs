using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Momente
{
    internal class Moment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateOnly CreationDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public TimeOnly CreationTime { get; set; } = TimeOnly.FromDateTime(DateTime.Now);
        public string? Icon { get; set; }
        public string? Headline { get; set; }
        public string? Description { get; set; }
        public Moment() { }
    }
}
