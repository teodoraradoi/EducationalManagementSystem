using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtCore.Data.Entities.Abstractions;

namespace Secretaries.Data.Entities
{
    public class Secretary : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
