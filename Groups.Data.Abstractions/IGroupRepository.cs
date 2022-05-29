using ExtCore.Data.Abstractions;
using Groups.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groups.Data.Abstractions
{
    public interface IGroupRepository : IRepository
    {
        IEnumerable<Group> All();
        Group FindById(Guid? id);
        void Create(Group group);
        void Edit(Group group);
        void Delete(Guid id);
    }
}
