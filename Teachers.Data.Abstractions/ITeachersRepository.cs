using ExtCore.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teachers.Data.Entities;

namespace Teachers.Data.Abstractions
{
    public interface ITeachersRepository : IRepository
    {
        IEnumerable<Teacher> All();
        void Create(Teacher teacher);
        Teacher FindTeacherByUserId(Guid id);
        Teacher FindById(Guid id);
        void Update(Teacher teacher);
        void Delete(Guid id);
    }
}
