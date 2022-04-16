﻿using ExtCore.Data.Abstractions;
using Students.Data.Entities;
using System;
using System.Collections.Generic;

namespace Students.Data.Abstractions
{
    public interface IStudentRepository : IRepository
    {
        IEnumerable<Student> All();
    }
}