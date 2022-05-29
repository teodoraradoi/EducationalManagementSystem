using ExtCore.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignments.Data.Entities;

namespace Assignments.Data.Abstractions
{
    public interface IAssignmentRepository : IRepository
    {
        IEnumerable<Assignment> All();
        Assignment FindById(Guid? id);
        void Create(Assignment assignment);
        void Edit(Assignment assignment);
        void Delete(Guid id);
    }
}
