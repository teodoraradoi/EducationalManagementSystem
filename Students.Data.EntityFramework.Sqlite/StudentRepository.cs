using ExtCore.Data.EntityFramework;
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
    }
}
