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
            applicationBuilder.UseHsts();

            applicationBuilder.UseHttpsRedirection();
            applicationBuilder.UseStaticFiles();


            applicationBuilder.UseAuthentication();
            applicationBuilder.UseAuthorization();
        } 
    }
}
