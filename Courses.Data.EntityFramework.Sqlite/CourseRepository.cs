using Courses.Data.Abstractions;
using Courses.Data.Entities;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courses.Data.EntityFramework.Sqlite
{
    public class CourseRepository : RepositoryBase<Course>, ICourseRepository
    {
        public IEnumerable<Course> All()
        {
            return this.dbSet.OrderBy(p => p.Name);
        }

        public void Create(Course course)
        {
           this.dbSet.Add(course);
        }

        public void Edit(Course course)
        {
            this.storageContext.Entry(course).State = EntityState.Modified;
        }

        public Course FindById(Guid? id)
        {
            Course course = this.dbSet.FirstOrDefault(e => e.Id == id);
            ///Course course = this.dbSet.FirstOrDefault(e => e.Id == id);
            //return course;
            return this.dbSet.FirstOrDefault(e => e.Id == id);
        }

        public void Delete(Guid id)
        {
            this.dbSet.Remove((this.FindById(id)));
        }

        public IEnumerable<Course> GetAllByTeacherId(Guid id)
        {
            return this.dbSet.Where(l => l.TeacherId == id);
        }
    }
}
