using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtCore.Data.EntityFramework;
using Subjects.Data.Abstractions;
using Subjects.Data.Entities;

namespace Subjects.Data.EntityFramework.Sqlite
{
    public class SubjectRepository : RepositoryBase<Subject>, ISubjectRepository
    {
        public IEnumerable<Subject> All()
        {
            return this.dbSet.OrderBy(p => p.Name);
        }
    }
}
