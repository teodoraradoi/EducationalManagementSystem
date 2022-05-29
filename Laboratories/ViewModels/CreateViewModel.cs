using Courses.Data.Entities;
using Laboratories.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.ViewModels
{
    public class CreateViewModel
    {
        public Laboratory Laboratory { get; set; }
        public List<Course> Courses { get; set; }
    }
}
