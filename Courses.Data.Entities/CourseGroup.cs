using ExtCore.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courses.Data.Entities
{
    public class CourseGroup : IEntity
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public Guid GroupId { get; set; }

    }
}
