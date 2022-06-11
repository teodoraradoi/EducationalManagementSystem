using ExtCore.Data.EntityFramework;
using Laboratories.Data.Abstractions;
using Laboratories.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Data.EntityFramework.Sqlite
{
    public class LaboratoryRepository : RepositoryBase<Laboratory>, ILaboratoryRepository
    {
        public IEnumerable<Laboratory> All()
        {
            return this.dbSet.OrderBy(p => p.Name);
        }
        public Laboratory FindById(Guid? id)
        {
            return this.dbSet.FirstOrDefault(e => e.Id == id);
        }

        public void Create(Laboratory laboratory)
        {
            this.dbSet.Add(laboratory);
        }

        public void Edit(Laboratory laboratory)
        {
            this.storageContext.Entry(laboratory).State = EntityState.Modified;
        }

        public void Delete(Guid id)
        {
            this.dbSet.Remove((this.FindById(id)));
        }

        public IEnumerable<Laboratory> AllByUserId(Guid id)
        {
            return this.dbSet.Where(l => l.TeacherId == id);
        }

        public IEnumerable<Laboratory> GetAllByCourseId(Guid id)
        {
            return this.dbSet.Where(l => l.CourseId == id);
        }
    }
}
