using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WestMarchSite.Application;
using WestMarchSite.Infrastructure;

namespace WestMarchSite
{
    public static class DependencyInjectionContainer
    {
        public static void SetupDiContainer(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingletonImplicit<ISiteConfiguration>(config.Get<SiteConfiguration>());

            services.AddSingletonImplicit<SessionRepository>();
            services.AddSingletonImplicit<SessionService>();
        }



        //I'm really sick of explicit registration, this takes some intentional design of interface to concretes but I prefer it...

        public static IServiceCollection AddSingletonImplicit<T>(this IServiceCollection services)
            where T : class
        {
            foreach(var i in typeof(T).GetInterfaces())
            {
                services.AddSingleton(i, typeof(T));
            }
            return services;
        }

        public static IServiceCollection AddSingletonImplicit<T>(this IServiceCollection services, object concrete)
            where T : class
        {
            foreach (var i in typeof(T).GetInterfaces())
            {
                services.AddSingleton(i, concrete);
            }
            return services;
        }
    }
}
