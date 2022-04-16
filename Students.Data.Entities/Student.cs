using ExtCore.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students.Data.Entities
{
    public class Student : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
