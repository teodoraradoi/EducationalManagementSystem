using ExtCore.Mvc.Infrastructure.Actions;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barebone.Actions
{
    public class UseEndpointAction : IUseEndpointsAction
    {
        public int Priority => 1000;

        public void Execute(IEndpointRouteBuilder endpointRouteBuilder, IServiceProvider serviceProvider)
        {
            endpointRouteBuilder.MapControllerRoute(name: "Default", pattern: "{controller}/{action}", 
                defaults: new { controller = "Default", action = "Index" });
        }
    }
}
