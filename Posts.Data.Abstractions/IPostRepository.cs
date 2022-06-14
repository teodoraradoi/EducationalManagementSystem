using ExtCore.Data.Abstractions;
using Posts.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posts.Data.Abstractions
{
    public interface IPostRepository: IRepository
    {
        public IEnumerable<Post> All();
        public Post FindById(Guid id);
        public void Create(Post post);
        public void Edit(Post post);
        public void Delete(Guid id);
        public IEnumerable<Post> AllBySubjectId(Guid id);

    }
}
