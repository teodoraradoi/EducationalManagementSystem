using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Secretaries.Data.Entities;

namespace Secretaries.Data.Abstractions
{
    public interface ISecretaryRepository : IRepository
    {
        IEnumerable<Secretary> All();
        void Create(Secretary secretary);
        //Teacher FindTeacherByUserId(Guid id);
        Secretary FindById(Guid id);
        void Update(Secretary secretary);
        void Delete(Guid id);
    }
}
