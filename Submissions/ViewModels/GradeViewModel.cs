using Assignments.Data.Entities;
using Students.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submissions.ViewModels
{
    public class GradeViewModel
    {
        public Guid SubmissionId { get; set; }
        public Student student { get; set; }
        public Assignment assignment { get; set; }
        public int Grade { get; set; }
    }
}
