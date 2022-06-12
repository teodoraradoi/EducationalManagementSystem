using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students.ViewModels
{
    public class StudentAccountViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
        public string GroupId { get; set; }
        public string SubgroupId { get; set; }
    }
}
