using ExtCore.Data.Abstractions;
using Submissions.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submissions.Data.Abstractions
{
    public interface ISubmissionRepository : IRepository
    {
        IEnumerable<Submission> All();
        Submission FindById(Guid? id);
        void Create(Submission submission);
        void Edit(Submission submission);
        void Delete(Guid id);
        List<Submission> AllByAssignmentId(Guid id);
        public Submission FindByStudentAndAssignmentId(Guid studentId, Guid assignmentId);
    }
}
