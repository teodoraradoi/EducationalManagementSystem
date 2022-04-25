using ExtCore.Infrastructure.Actions;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barebone.Actions
{
    public class UseAuthorizationAction : IConfigureAction
    {
        public int Priority => 10001;

        public void Execute(IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
        {
         
                //applicationBuilder.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            applicationBuilder.UseHsts();

            applicationBuilder.UseHttpsRedirection();
            applicationBuilder.UseStaticFiles();




            applicationBuilder.UseAuthorization();
            applicationBuilder.UseAuthentication();
        } 
    }
}
