using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Students.Data.Abstractions;
using Students.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students.Data.EntityFramework.Sqlite
{
    public class StudentRepository : RepositoryBase<Student>, IStudentRepository
    {
        public IEnumerable<Student> All()
        {
            return this.dbSet.OrderBy(p => p.Id);
        }

        public void Create(Student student)
        {
            this.dbSet.Add(student);
        }

        public void Delete(Guid id)
        {
            this.dbSet.Remove((this.FindById(id)));
        }

        public Student FindById(Guid id)
        {
            return this.dbSet.FirstOrDefault(s => s.Id == id);
        }

        public Student FindByUserId(Guid id)
        {
            return this.dbSet.FirstOrDefault(s => s.UserId == id);
        }

        public void Update(Student student)
        {
            this.storageContext.Entry(student).State = EntityState.Modified;
        }
    }
}
