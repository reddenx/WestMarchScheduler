using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WestMarchSite.Core
{
    public class SessionEntity
    {
        public string DmKey { get; }
        public string LeadKey { get; }
        public string PlayerKey { get; }

        public string Title { get; internal set; }
        public string Description { get; internal set; }

        public string LeadName { get; internal set; }

        public bool IsValid => throw new NotImplementedException();
    }
}
