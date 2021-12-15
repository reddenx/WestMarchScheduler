using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WestMarchSite.Infrastructure
{
    public interface ISiteConfiguration : ISessionRepositoryConfiguration
    { }
    public class SiteConfiguration : ISiteConfiguration
    {
        public string SessionDbConnectionString => ConnectionStrings?.Session;

        public ConnectionStringObj ConnectionStrings { get; set; }



        public class ConnectionStringObj
        {
            public string Session { get; set; }
        }
    }
}
