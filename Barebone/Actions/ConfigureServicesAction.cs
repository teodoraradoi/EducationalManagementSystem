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
            // This is a bad (but quick) way to provide configurations to the extensions. A good one is to use the options pattern.
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
              .SetBasePath(serviceProvider.GetService<IWebHostEnvironment>().ContentRootPath)
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlite(configurationBuilder.Build().GetConnectionString("Default")));


          

            serviceCollection.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddRoles<ApplicationRole>()
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders().AddDefaultUI();


            /*serviceCollection.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();*/



           // serviceCollection.AddDefaultIdentity<ApplicationUser>().AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();



            /*serviceCollection.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
               .AddRoles<ApplicationRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>();*/





            serviceCollection.AddScoped(typeof(IStorageContext), typeof(ApplicationDbContext));

            //var userManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            //await this.CreateRoles(serviceProvider);



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
                    //new IdentityRole(roleName)
                }
            }

            //Here you could create a super user who will maintain the web app
            /*var poweruser = new ApplicationUser
            {

                UserName = Configuration["AppSettings:UserName"],
                Email = Configuration["AppSettings:UserEmail"],
            };
            //Ensure you have these values in your appsettings.json file
            string userPWD = Configuration["AppSettings:UserPassword"];
            var _user = await UserManager.FindByEmailAsync(Configuration["AppSettings:AdminUserEmail"]);

            if (_user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the role
                    await UserManager.AddToRoleAsync(poweruser, "Admin");

                }
            }*/
        }



    }
}
