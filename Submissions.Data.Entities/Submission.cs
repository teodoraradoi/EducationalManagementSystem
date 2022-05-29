using ExtCore.Data.Entities.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Submissions.Data.Entities
{
    public class Submission : IEntity
    {
        public Guid Id { get; set; }
        //[NotMapped]
        public string FilePath { get; set; }
        public string Status { get; set; }
        public Guid AssignmentId { get; set; }
        public DateTime TurnedInDate { get; set; }  
        public int Grade { get; set; } 

        // public Student 
    }
}
