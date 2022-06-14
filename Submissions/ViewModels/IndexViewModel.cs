using Assignments.Data.Entities;
using Courses.Data.Entities;
using Laboratories.Data.Entities;
using Students.Data.Entities;
using Submissions.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submissions.ViewModels
{
    public class IndexViewModel
    {
        public List<Tuple<Submission, Assignment, Course, Student>> courseSubmissions { get; set; }
        public List<Tuple<Submission, Assignment, Laboratory, Student>> laboratorySubmissions { get;set; }
    }
}
