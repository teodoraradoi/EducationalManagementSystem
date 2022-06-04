using Courses.Data.Abstractions;
using Courses.Data.Entities;
using ExtCore.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courses.Data.EntityFramework.Sqlite
{
    public class CourseGroupRepository : RepositoryBase<CourseGroup>, ICourseGroupRepository
    {
    }
}
