using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignments.Data.Entities;
using Assignments.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Assignments.Data.EntityFramework.Sqlite
{
    public class AssignmentRepository : RepositoryBase<Assignment>, IAssignmentRepository
    {
        public IEnumerable<Assignment> All()
        {
            return this.dbSet.OrderBy(p => p.Name);
        }

        public void Create(Assignment assignment)
        {
            this.dbSet.Add(assignment);
        }
        public Assignment FindById(Guid? id)
        {
            return this.dbSet.FirstOrDefault(e => e.Id == id);
        }

        public void Edit(Assignment assignment)
        {
            this.storageContext.Entry(assignment).State = EntityState.Modified;
        }
        public void Delete(Guid id)
        {
            this.dbSet.Remove((this.FindById(id)));
        }
    }
}
