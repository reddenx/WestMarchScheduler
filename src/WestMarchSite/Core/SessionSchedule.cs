using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WestMarchSite.Core
{
    public class SessionSchedule
    {
        public SessionScheduleOption[] Options { get; private set; }

        public SessionSchedule(SessionScheduleOption[] options)
        {
            Options = options;
        }
    }
    public class SessionScheduleOption
    {
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public SessionScheduleOption(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }
    }
}
