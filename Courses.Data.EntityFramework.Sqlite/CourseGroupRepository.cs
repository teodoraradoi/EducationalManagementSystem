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
    public class CourseGroupRepository : RepositoryBase<CourseGroup>, ICourseGroupRepository
    {
        public void Create(List<CourseGroup> courseGroups)
        {
            this.dbSet.AddRangeAsync(courseGroups);
        }

        public CourseGroup GetGroupByCourseId(Guid id)
        {
            return this.dbSet.FirstOrDefault(c => c.CourseId == id);
        }

        public IEnumerable<CourseGroup> GetByGroupId(Guid id)
        {
            return this.dbSet.Where(c => c.GroupId == id);
        }

        public List<CourseGroup> AllByCourseId(Guid id)
        {
            List<CourseGroup> courseGroups = new List<CourseGroup>();
            courseGroups = this.dbSet.AsNoTracking().
                Where(c => c.CourseId == id).ToList();
            return courseGroups;
        }

        public IEnumerable<CourseGroup> All()
        {
            return this.dbSet.OrderBy(t => t.Id);
        }

        public void Delete(CourseGroup[] courseGroups)
        {
            this.dbSet.RemoveRange(courseGroups);
        }
    }
}
