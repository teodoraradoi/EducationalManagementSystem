using Assignments.Data.Entities;
using Students.Data.Entities;
using Submissions.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignments.ViewModels
{
    public class DetailsForTeacherViewModel
    {
        public Assignment assignment { get; set; }
        public List<Tuple<Submission, Student>> submissionsList { get; set; }
    }
}
