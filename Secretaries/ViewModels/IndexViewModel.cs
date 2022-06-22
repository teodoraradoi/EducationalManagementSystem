using Identity.Data.Entities;
using Secretaries.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Secretaries.ViewModels
{
    public class IndexViewModel
    {
        public Secretary secretary { get; set; }
        public ApplicationUser user { get; set; }
    }
}
