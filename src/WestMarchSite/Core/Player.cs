using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WestMarchSite.Core
{
    public class Player
    {
        public SessionSchedule Schedule { get; private set; }
        public string Name { get; private set; }

        public Player(string name, SessionSchedule schedule)
        {
            Schedule = schedule;
            Name = name;
        }
    }
}
