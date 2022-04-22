using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Identity.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Data.EntityFramework
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>, IStorageContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            this.RegisterEntities(modelBuilder);

            /*string ROLE_ID = "341743f0 - asd2–42de - afbf - 59kmkkmk72cf6";

            //seed admin role
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "Secretary",
                NormalizedName = "SECRETARY",
                Id = ROLE_ID,
                ConcurrencyStamp = ROLE_ID
            });*/
         
        }
    }
}
