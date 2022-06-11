using Courses.Data.Entities;
using ExtCore.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Data.Entities
{
    public class Laboratory : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DayOfWeek Day { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime Time { get; set; }

        public Guid CourseId { get; set; }
        public Guid SubgroupId { get; set; }

        public Guid TeacherId { get; set; }
    }
}
