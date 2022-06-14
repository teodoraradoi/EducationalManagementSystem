using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Posts.Data.Abstractions;
using Posts.Data.Entities;

namespace Posts.Data.EntityFramework.Sqlite
{
    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        public IEnumerable<Post> All()
        {
            return this.dbSet.OrderBy(p => p.Name);
        }

        public IEnumerable<Post> AllBySubjectId(Guid id)
        {
           return this.dbSet.Where(p => p.SubjectId == id);
        }

        public void Create(Post post)
        {
            this.dbSet.Add(post);
        }

        public void Delete(Guid id)
        {
            this.dbSet.Remove(this.FindById(id));
        }

        public void Edit(Post post)
        {
            this.storageContext.Entry(post).State = EntityState.Modified;
        }

        public Post FindById(Guid id)
        {
            return this.dbSet.FirstOrDefault(p => p.Id == id);
        }
    }
}
