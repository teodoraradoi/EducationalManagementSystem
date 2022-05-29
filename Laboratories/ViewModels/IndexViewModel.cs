using Courses.Data.Entities;
using Laboratories.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.ViewModels
{
    public class IndexViewModel
    {
        public Laboratory Laboratory { get; set; }
        public Course Course { get; set; }
    }
}
