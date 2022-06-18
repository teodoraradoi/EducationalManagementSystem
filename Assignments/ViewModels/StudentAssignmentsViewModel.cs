using Assignments.Data.Entities;
using Courses.Data.Entities;
using Laboratories.Data.Entities;
using Submissions.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignments.ViewModels
{
    public class StudentAssignmentsViewModel
    {
        public List<Tuple<Assignment, Course, Submission>> courseAssignments { get; set; }
        public List<Tuple<Assignment, Laboratory, Submission>> laboratoryAssignments { get; set; }
    }
}
