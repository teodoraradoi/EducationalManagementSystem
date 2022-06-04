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
        IEnumerable<Subgroup> AllById(Guid id);
        Subgroup FindById(Guid? id);
        void Create(Subgroup subgroup);
        void Edit(Subgroup subgroup);
        void Delete(Guid id);
    }
}
