using ExtCore.Data.Abstractions;
using Laboratories.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratories.Data.Abstractions
{
    public interface ILaboratoryRepository : IRepository
    {
        IEnumerable<Laboratory> All();
        Laboratory FindById(Guid? id);
        IEnumerable<Laboratory> AllByUserId(Guid id);
        void Create(Laboratory laboratory);
        void Edit(Laboratory laboratory);
        void Delete(Guid id);

        IEnumerable<Laboratory> GetAllByCourseId(Guid id);
    }
}
