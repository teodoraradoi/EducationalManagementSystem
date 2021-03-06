using ExtCore.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groups.Data.Entities
{
    public class Group : IEntity
    {
        public Guid Id { get; set; }
        public string SpecializationName { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
    }
}
