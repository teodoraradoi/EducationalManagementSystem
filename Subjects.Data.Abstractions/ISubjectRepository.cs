using ExtCore.Data.Abstractions;
using Subjects.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subjects.Data.Abstractions
{
    public interface ISubjectRepository : IRepository
    {
        IEnumerable<Subject> All();
    }
}
