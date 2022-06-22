using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
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

        public void Create(Secretary secretary)
        {
            this.dbSet.Add(secretary);
        }

        public void Delete(Guid id)
        {
            this.dbSet.Remove((this.FindById(id)));
        }

        public Secretary FindById(Guid id)
        {
            return this.dbSet.FirstOrDefault(p => p.Id == id);
        }

        public void Update(Secretary secretary)
        {
            this.storageContext.Entry(secretary).State = EntityState.Modified;
        }
    }
}
