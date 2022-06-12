using ExtCore.Data.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posts.Data.Entities
{
    public class Post: IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid SubjectId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
