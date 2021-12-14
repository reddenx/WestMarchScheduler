using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WestMarchSite.Infrastructure
{
    public interface ISiteLogger
    {
        void Error(string message);
        void Error(Exception error);
        void Warning(string message);
        void Debug(string message);
    }
}
