using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WestMarchSite.Core
{
    public class SessionEntity
    {
        public string HostKey { get; }
        public string LeadKey { get; }
        public string PlayerKey { get; }

        public string Title { get; internal set; }
        public string Description { get; internal set; }

        public string LeadName { get; internal set; }

        public string HostName { get; internal set; }
        public SessionSchedule HostSchedule { get; internal set; }

        public bool IsValid => throw new NotImplementedException();

        internal void SetHostSchedule(SessionSchedule[] schedule)
        {
            throw new NotImplementedException();
        }

        internal void SetLeadSchedule(SessionSchedule[] sessionSchedules)
        {
            throw new NotImplementedException();
        }
    }
}
