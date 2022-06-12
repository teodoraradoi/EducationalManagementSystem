using Courses.Data.Entities;
using ExtCore.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courses.Data.Abstractions
{
    public interface ICourseGroupRepository : IRepository
    {
        CourseGroup GetGroupByCourseId(Guid id);
        void Create(List<CourseGroup> courseGroup);
        IEnumerable<CourseGroup> GetByGroupId(Guid id);
    }
}
