using Courses.Data.Entities;
using Groups.Data.Entities;
using Identity.Data.Entities;
using Laboratories.Data.Entities;
using Students.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Students.ViewModels
{
    public class DetailsViewModel
    {
        public Student student { get; set; }
        public ApplicationUser user { get; set; }
        public List<Course> courses { get; set; }
        public List<Tuple<Laboratory, Course>> laboratories { get; set; }
    }
}
