using System;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Students.Data.Entities;

namespace Students.Data.EntityFramework.Sqlite
{
    public class EntityRegistrar : IEntityRegistrar
    {
        public void RegisterEntities(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<Student>(etb =>
            {
                etb.HasKey(e => e.Id);
                etb.Property(e => e.Id);
                etb.ToTable("Student");
            });
        }
    }
}
