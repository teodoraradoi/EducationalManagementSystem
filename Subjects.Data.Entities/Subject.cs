using ExtCore.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subjects.Data.Entities
{
    public class Subject : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int CourseId { get; set; }
        public int LaboratoryId { get; set; }
        public int Year { get; set; }
        public int Semester { get; set; }

    }
}
