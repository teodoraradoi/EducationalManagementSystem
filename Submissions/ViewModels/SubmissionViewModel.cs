using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submissions.ViewModels
{
    public class SubmissionViewModel
    {
        public Guid Id { get; set; }
      
        public IFormFile FilePath { get; set; }
        public string Status { get; set; }
        public Guid AssignmentId { get; set; }
        public DateTime TurnedInDate { get; set; }
        public int Grade { get; set; }

        // public Student 
    }
}
