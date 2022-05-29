using Groups.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groups.ViewModels
{
    public class CreateViewModel
    {
        public Subgroup Subgroup { get; set; }
        public List<Group> Groups { get; set; }
    }
}
