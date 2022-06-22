using Courses.Data.Entities;
using Groups.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teachers.Data.Entities;

namespace Courses.ViewModels
{
    public class CourseViewModel
    {
        public Course course { get; set; }
       public IEnumerable<SelectListItem> groups { get; set; }

        public IEnumerable<string> selectedGroups { get; set; }
        public List<Teacher> Teachers { get; set; }
    }
}
