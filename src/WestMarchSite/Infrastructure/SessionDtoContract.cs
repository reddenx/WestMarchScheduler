using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WestMarchSite.Infrastructure
{
    public class CreateSessionDto
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class CreateSessionResultDto
    {
        public string HostKey { get; set; }
        public string LeadKey { get; set; }
        public string PlayerKey { get; set; }
    }

    public class ApproveSessionDto
    {
        public string Name { get; set; }
        public SessionScheduleDateDto[] Schedule { get; set; }
    }

    public class ApproveSessionResultDto
    {
        public string HostKey { get; set; }
    }

    public class LeadScheduleDto
    {
        public SessionScheduleDateDto[] Schedule { get; set; }
    }

    public class LeadScheduleResultDto
    {
        public string LeadKey { get; set; }
        public string HostKey { get; set; }
        public string PlayerKey { get; set; }
    }

    public class PlayerJoinDto
    {
        public string Name { get; set; }
        public SessionScheduleDateDto[] Schedule { get; set; }
    }

    public class HostFinalizeDto
    {
        public SessionScheduleDateDto[] Schedule { get; set; }
    }

    public class SessionDto
    {
        //probably best to keep these only on the entity
        public string PlayerKey { get; set; }
        public string HostKey { get; set; }
        public string LeadKey { get; set; }

        public SessionStatusDto Status { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        //public DateTime PostDate { get; set; }

        public PlayerDto Lead { get; set; }
        public PlayerDto Host { get; set; }
        public PlayerDto[] Players { get; set; }

        public SessionScheduleDateDto[] FinalizedSchedule { get; set; }

        public class PlayerDto
        {
            public string Name { get; set; }
            public SessionScheduleDateDto[] Schedule { get; set; }
        }

        public enum SessionStatusDto
        {
            /// <summary>
            /// idea has been posted but unapproved by Host
            /// </summary>
            Posted,
            /// <summary>
            /// Host has approved and set their schedule, waiting for leader schedule selection
            /// </summary>
            Approved,
            /// <summary>
            /// Session is open for player joining
            /// </summary>
            Open,
            /// <summary>
            /// Session has been closed to joining
            /// </summary>
            Finalized
        }
    }

    public class SessionScheduleDateDto
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
