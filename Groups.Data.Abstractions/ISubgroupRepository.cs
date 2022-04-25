using ExtCore.Data.Abstractions;
using Groups.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groups.Data.Abstractions
{
    public interface ISubgroupRepository : IRepository
    {
        IEnumerable<Subgroup> All();
        void Create(Subgroup subgroup);
    }
}
