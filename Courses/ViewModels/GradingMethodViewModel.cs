using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courses.ViewModels
{
    public class GradingMethodViewModel
    {
        public Guid id { get; set; }
        public int FinalExamPercent { get; set; }
        public int LaboratoryPercent { get; set; }
        public bool AttendanceMatters { get; set; }
    }
}
