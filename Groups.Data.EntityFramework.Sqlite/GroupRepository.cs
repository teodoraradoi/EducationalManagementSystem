using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groups.Data.Entities;
using Groups.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Groups.Data.EntityFramework.Sqlite
{
    public class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        public IEnumerable<Group> All()
        {
            return this.dbSet.OrderBy(p => p.Name);
        }

        public Group FindById(Guid? id)
        {
            return this.dbSet.FirstOrDefault(e => e.Id == id);
        }

        public void Create(Group group)
        {
            this.dbSet.Add(group);
        }
        public void Edit(Group group)
        {
            this.storageContext.Entry(group).State = EntityState.Modified;
        }

        public void Delete(Guid id)
        {
            this.dbSet.Remove((this.FindById(id)));
        }
    }
}
