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

namespace Courses.ViewModels
{
    public class DetailsForTeacherViewModel
    {
        public Course course { get; set; }
        public List<Laboratory> laboratories { get; set; }
        public List<Assignment> assignments { get; set; }
        public List<Post> posts { get; set; }
        public List <Group> groups { get; set; }
    }
}
