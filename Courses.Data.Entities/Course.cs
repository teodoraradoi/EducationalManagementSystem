using ExtCore.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courses.Data.Entities
{
    public class Course : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public DayOfWeek Day { get; set; }

        public DateTime Time { get; set; }
        public int Year { get; set; }
        public int Semester { get; set; }
        //public List<Groups> Groups { get; set; }
    }
}
