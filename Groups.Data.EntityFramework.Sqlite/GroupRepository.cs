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
    public class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        public IEnumerable<Group> All()
        {
            return this.dbSet.OrderBy(p => p.Name);
        }

        public void Create(Group group)
        {
            this.dbSet.Add(group);
        }
    }
}
