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
    public class CreateViewModel
    {
        public Laboratory Laboratory { get; set; }
        public List<Subgroup> Subgroups { get; set; }
        public List<Teacher> Teachers { get; set; }
    }
}
