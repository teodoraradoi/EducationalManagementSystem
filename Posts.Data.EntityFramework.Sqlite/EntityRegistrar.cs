using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Posts.Data.Entities;

namespace Posts.Data.EntityFramework.Sqlite
{
    public class EntityRegistrar : IEntityRegistrar
    {
        public void RegisterEntities(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<Post>(etb =>
            {
                etb.HasKey(e => e.Id);
                etb.Property(e => e.Id);
                etb.ToTable("Post");
            });
        }
    }
}
