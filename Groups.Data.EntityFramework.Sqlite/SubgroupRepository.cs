using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groups.Data.Entities;
using Groups.Data.Abstractions;
using ExtCore.Data.EntityFramework;

namespace Groups.Data.EntityFramework.Sqlite
{
    internal class SubgroupRepository : RepositoryBase<Subgroup>, ISubgroupRepository
    {
        public IEnumerable<Subgroup> All()
        {
            return this.dbSet.OrderBy(p => p.Name);
        }

        public void Create(Subgroup subgroup)
        {
            this.dbSet.Add(subgroup);
        }
    }
}
