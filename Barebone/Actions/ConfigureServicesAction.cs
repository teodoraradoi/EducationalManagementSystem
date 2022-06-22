using System;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using ExtCore.Infrastructure.Actions;
using Identity.Data.Entities;
using Identity.Data.EntityFramework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Barebone
{
    public class ConfigureServicesAction : IConfigureServicesAction
    {
        public int Priority => 1000;

        public void Execute(IServiceCollection serviceCollection, IServiceProvider serviceProvider)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
              .SetBasePath(serviceProvider.GetService<IWebHostEnvironment>().ContentRootPath)
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlite(configurationBuilder.Build().GetConnectionString("Default")));

            serviceCollection.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddRoles<ApplicationRole>()
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders().AddDefaultUI();


            serviceCollection.AddScoped(typeof(IStorageContext), typeof(ApplicationDbContext));

        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {        
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "Secretary", "Teacher", "Student" };
            IdentityResult roleResult;

            int i = 1;
            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new ApplicationRole());                 
                }
            }
        }
    }
}
