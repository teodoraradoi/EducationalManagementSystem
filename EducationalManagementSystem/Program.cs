using Identity.Data.EntityFramework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EducationalManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {

           // MainAsync(args).GetAwaiter().GetResult();

            CreateHostBuilder(args).Build().Run();
            //IHost host = CreateHostBuilder(args).Build();



            /*var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));


            if (!roleManager.RoleExistsAsync("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "ROLE NAME";
                roleManager.CreateAsync(role);

            }*/


            /*using (var scope = host.Services.CreateScope())
             {
                 var services = scope.ServiceProvider;

                using (var manager = services.GetRequiredService<RoleManager<IdentityRole>>())
                {




                     //RoleManager<IdentityRole> manager = services.GetService<RoleManager<IdentityRole>>();
                    //var manager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(ApplicationDbContext));




                    if (!manager.RoleExistsAsync("Secretary").Result)
                    {
                        var result = manager.CreateAsync(new IdentityRole("Secretary")).Result;
                    }

                    if (!manager.RoleExistsAsync("Teacher").Result)
                    {
                        var result = manager.CreateAsync(new IdentityRole("Teacher")).Result;
                    }

                    UserManager<IdentityUser> userManager = services.GetService<UserManager<IdentityUser>>();

                    var user = userManager.FindByEmailAsync("admin@gmail.com").Result;
                    if (user != null)
                    {
                        if (!userManager.IsInRoleAsync(user, "Secretary").Result)
                        {
                            var addResult = userManager.AddToRoleAsync(user, "Secretary").Result;
                        }
                    }
                }
             }*/
            //host.Run();








            /*   new WebHostBuilder()
                .UseKestrel()
               .UseContentRoot(Directory.GetCurrentDirectory())
               .UseStartup<Startup>()
               .Build()
                .Run();*/
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
