using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teachers.Data.Abstractions;
using Teachers.Data.Entities;

namespace Teachers.Data.EntityFramework.Sqlite
{
    public class TeachersRepository : RepositoryBase<Teacher>, ITeachersRepository
    {
        public IEnumerable<Teacher> All()
        {
            return this.dbSet.OrderBy(p => p.Id);
        }

        public void Create(Teacher teacher)
        {
            this.dbSet.Add(teacher);
        }

        public void Delete(Guid id)
        {
            this.dbSet.Remove((this.FindById(id)));
        }

        public Teacher FindById(Guid id)
        {
            return this.dbSet.FirstOrDefault(p => p.Id == id);
        }

        public Teacher FindTeacherByUserId(Guid id)
        {
            return this.dbSet.FirstOrDefault(p => p.UserId == id);
        }

        public void Update(Teacher teacher)
        {
            this.storageContext.Entry(teacher).State = EntityState.Modified;
        }
    }

}
