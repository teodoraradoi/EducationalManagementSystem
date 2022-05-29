using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Courses.Data.Entities;
using ExtCore.Data.Abstractions;

namespace Courses.Data.Abstractions
{
    public interface ICourseRepository : IRepository
    {
        IEnumerable<Course> All();
        Course FindById(Guid? id);
        void Create(Course course);
        void Edit(Course course);
        void Delete(Guid id);
    }
}
