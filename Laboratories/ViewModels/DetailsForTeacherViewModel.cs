using Assignments.Data.Entities;
using Courses.Data.Entities;
using Groups.Data.Entities;
using Laboratories.Data.Entities;
using Posts.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.ViewModels
{
    public class DetailsForTeacherViewModel
    {
        public Laboratory laboratory { get; set; }
        public Course course { get; set; }
        public List<Assignment> assignments { get; set; }
        public List<Post> posts { get; set; }
        public Tuple<Group, Subgroup> group{ get; set; }
    }
}
