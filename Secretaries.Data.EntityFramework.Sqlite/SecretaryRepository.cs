using ExtCore.Data.EntityFramework;
using Secretaries.Data.Abstractions;
using Secretaries.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Secretaries.Data.EntityFramework.Sqlite
{
    public class SecretaryRepository : RepositoryBase<Secretary>, ISecretaryRepository
    {
        public IEnumerable<Secretary> All()
        {
            return this.dbSet.OrderBy(p => p.Name);
        }
    }
}
