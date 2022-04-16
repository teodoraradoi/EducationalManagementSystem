using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teachers.Data.Entities;

namespace Teachers.Data.EntityFramework.Sqlite
{
    public class EntityRegistrar : IEntityRegistrar
    {
        public void RegisterEntities(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<Teacher>(etb =>
            {
                etb.HasKey(e => e.Id);
                etb.Property(e => e.Id);
                etb.ToTable("Teacher");
            });
        }
    }
}
