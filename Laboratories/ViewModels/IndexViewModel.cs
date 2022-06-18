using Courses.Data.Entities;
using Groups.Data.Entities;
using Laboratories.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teachers.Data.Entities;

namespace Laboratories.ViewModels
{
    public class IndexViewModel
    {
        public Laboratory laboratory { get; set; }
        public Course course { get; set; }
        public Group  group { get; set; }
        public Subgroup subgroup { get; set; }
        public Teacher teacher { get; set; }
    }
}
