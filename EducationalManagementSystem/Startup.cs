using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ExtCore.WebApplication.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtCore.Infrastructure;
using Microsoft.Extensions.FileProviders;
using System.IO;
using ExtCore.Data.EntityFramework;
using Secretaries.Data.Abstractions;
using Secretaries.Data.EntityFramework.Sqlite;
using Microsoft.AspNetCore.Mvc;
using Teachers.Data.Abstractions;
using Teachers.Data.EntityFramework.Sqlite;
using Microsoft.AspNetCore.Identity;
using Identity.Data.Entities;

namespace EducationalManagementSystem
{
    public class Startup
    {
        private string extensionsPath;

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            this.Configuration = configuration;
            this.extensionsPath = webHostEnvironment.ContentRootPath + configuration["Extensions:Path"];
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();
            //services.AddControllers().AddApplicationPart();

            services.AddExtCore(this.extensionsPath);
            services.Configure<StorageContextOptions>(options =>
            {
                options.ConnectionString = this.Configuration.GetConnectionString("Default");
            }
             );

            //services.AddIdentity<>
            //services.AddScoped<ISecretaryRepository, SecretaryRepository>();
            //services.AddScoped<ITeachersRepository, TeachersRepository>();
            //services.AddScoped<ISubjectRepository, SubjectRepository>();

            //var serviceProvider = services.BuildServiceProvider();

            // await this.CreateRolesandUsers(serviceProvider);

        }


        /*private async Task CreateRolesandUsers(IServiceProvider service)
        {
            var _roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
            var _userManager = service.GetRequiredService<UserManager<ApplicationUser>>();

            bool x = await _roleManager.RoleExistsAsync("Secretary");
            if (!x)
            {
                // first we create Admin rool    
                var role = new IdentityRole();
                role.Name = "Secretary";
                await _roleManager.CreateAsync(role);

                //Here we create a Admin super user who will maintain the website                   

                var user = new ApplicationUser();
                user.UserName = "admin";
                user.Email = "admin@gmail.com";

                string userPWD = "somepassword";

                IdentityResult chkUser = await _userManager.CreateAsync(user, userPWD);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = await _userManager.AddToRoleAsync(user, "Admin");
                }
            }

            // creating Creating Manager role     
            x = await _roleManager.RoleExistsAsync("Teacher");
            if (!x)
            {
                var role = new IdentityRole();
                role.Name = "Teacher";
                await _roleManager.CreateAsync(role);
            }

            // creating Creating Employee role     
            x = await _roleManager.RoleExistsAsync("Student");
            if (!x)
            {
                var role = new IdentityRole();
                role.Name = "Student";
                await _roleManager.CreateAsync(role);
            }
        }*/




        public void Configure(IApplicationBuilder applicationBuilder)
        {
            
               


            applicationBuilder.UseDeveloperExceptionPage();

            applicationBuilder.UseExtCore();



            //applicationBuilder.UseRouting();

             //applicationBuilder.UseAuthentication();
            
            //applicationBuilder.UseAuthorization();




            applicationBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Login}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        /*
        private string extensionsPath;

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            this.extensionsPath = webHostEnvironment.ContentRootPath + configuration["Extensions:Path"];
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddExtCore(this.extensionsPath);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });

            app.UseExtCore();
        }*/
    }
}
