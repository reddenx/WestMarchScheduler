using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WestMarchSite.Infrastructure;

namespace WestMarchSite.Core
{
    public class SessionEntity
    {
        public string HostKey { get; }
        public string LeadKey { get; }
        public string PlayerKey { get; }

        public string Title { get; }
        public string Description { get; }

        public string LeadName { get; }

        public string HostName { get; }
        public SessionSchedule HostSchedule { get; }

        public bool IsValid => throw new NotImplementedException();

        public void SetInfo(string title, string description)
        {
            throw new NotImplementedException();
        }

        public void SetLead(string leadName)
        {
            throw new NotImplementedException();
        }

        public void SetHost(string hostName)
        {
            throw new NotImplementedException();
        }

        public void SetHostSchedule(SessionSchedule[] schedule)
        {
            throw new NotImplementedException();
        }

        public void SetLeadSchedule(SessionSchedule[] sessionSchedules)
        {
            throw new NotImplementedException();
        }

        public void AddPlayer(string name, SessionSchedule[] schedule)
        {
            throw new NotImplementedException();
        }

        public void SetFinalSchedule(HostFinalizeDto finalizeDto)
        {
            throw new NotImplementedException();
        }

        public void Finalize()
        {
            throw new NotImplementedException();
        }
    }
}
