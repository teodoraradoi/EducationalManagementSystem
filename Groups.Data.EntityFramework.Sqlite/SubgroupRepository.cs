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
    public class SubgroupRepository : RepositoryBase<Subgroup>, ISubgroupRepository
    {
        public IEnumerable<Subgroup> All()
        {
            return this.dbSet.OrderBy(p => p.Name);
        }
        public IEnumerable<Subgroup> AllById(Guid id)
        {
            IEnumerable<Subgroup> subgroups= this.dbSet.Where(p => p.GroupId == id);
            return subgroups;
        }
        public Subgroup FindById(Guid? id)
        {
            return this.dbSet.FirstOrDefault(e => e.Id == id);
        }
        public void Create(Subgroup subgroup)
        {
            this.dbSet.Add(subgroup);
        }
        public void Edit(Subgroup subgroup)
        {
            this.storageContext.Entry(subgroup).State = EntityState.Modified;
        }
        public void Delete(Guid id)
        {
            this.dbSet.Remove((this.FindById(id)));
        }

        public List<Subgroup> AllByGroupId(Guid id)
        {
            return this.dbSet.AsNoTracking().Where(s => s.GroupId == id).ToList();
        }
    }
}
