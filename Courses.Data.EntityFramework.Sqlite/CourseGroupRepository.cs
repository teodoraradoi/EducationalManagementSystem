﻿using Courses.Data.Abstractions;
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


    }
}
