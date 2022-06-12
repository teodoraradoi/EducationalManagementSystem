using Assignments.Data.Entities;
using Courses.Data.Entities;
using Laboratories.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignments.ViewModels
{
    public class AssignmentsViewModel
    {
        public List<Tuple<Assignment, Course>> coursesAssignments { get; set; }
        public List<Tuple<Assignment,Laboratory>> laboratoriesAssignments { get; set; }

        /*public Assignment Assignment { get; set; }
        public Course Course { get; set; }
        public Laboratory Laboratory { get; set; }*/

    }
}
