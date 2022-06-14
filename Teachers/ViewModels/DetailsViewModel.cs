using Courses.Data.Entities;
using Identity.Data.Entities;
using Laboratories.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teachers.Data.Entities;

namespace Teachers.ViewModels
{
    public class DetailsViewModel
    {
        public Teacher teacher { get; set; }
        public ApplicationUser user { get; set; }
        public List<Course> courses { get; set; }
        public List<Laboratory> laboratories { get; set; }
    }
}
