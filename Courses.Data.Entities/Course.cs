using ExtCore.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime Time { get; set; }
        public int Year { get; set; }
        public int Semester { get; set; }
        public Guid TeacherId { get; set; }
    }
}
