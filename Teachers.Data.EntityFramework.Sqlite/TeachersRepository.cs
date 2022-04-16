using ExtCore.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teachers.Data.Abstractions;
using Teachers.Data.Entities;

namespace Teachers.Data.EntityFramework.Sqlite
{
    public class TeachersRepository : RepositoryBase<Teacher>, ITeachersRepository
    {
        public IEnumerable<Teacher> All()
        {
            return this.dbSet.OrderBy(p => p.Name);
        }
    }

}
