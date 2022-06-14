using Assignments.Data.Entities;
using Courses.Data.Entities;
using Laboratories.Data.Entities;
using Posts.Data.Entities;
using Submissions.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teachers.Data.Entities;

namespace Laboratories.ViewModels
{
    public class DetailsForStudentViewModel
    {
        public Laboratory laboratory { get; set; }
        public Course course { get; set; }
        public List<Post> posts { get; set; }
        public Teacher teacher { get; set; }
        public List<Assignment> ToDo { get; set; }
        public List<Tuple<Assignment, Submission>> Done { get; set; }
    }
}
