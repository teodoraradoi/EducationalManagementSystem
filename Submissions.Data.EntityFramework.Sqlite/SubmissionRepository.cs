using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Submissions.Data.Abstractions;
using Submissions.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submissions.Data.EntityFramework.Sqlite
{
    public class SubmissionRepository : RepositoryBase<Submission>, ISubmissionRepository
    {
        public IEnumerable<Submission> All()
        {
            return this.dbSet.OrderBy(p => p.Id);
        }
        public Submission FindById(Guid? id)
        {
            return this.dbSet.FirstOrDefault(e => e.Id == id);
        }

        public void Create(Submission submission)
        {
            this.dbSet.Add(submission);
        }
        public void Edit(Submission submission)
        {
            this.storageContext.Entry(submission).State = EntityState.Modified;
        }

        public void Delete(Guid id)
        {
            this.dbSet.Remove((this.FindById(id)));
        }

        public List<Submission> AllByAssignmentId(Guid id)
        {
            return this.dbSet.Where(e => e.AssignmentId == id).ToList();
        }
    }
}
