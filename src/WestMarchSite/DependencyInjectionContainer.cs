using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WestMarchSite
{
    public static class DependencyInjectionContainer
    {
        public static void SetupDiContainer(this IServiceCollection services, IConfiguration config)
        { }
    }
}
