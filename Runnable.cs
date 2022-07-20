using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interpreterv2
{
    public class Runnable
    {
        public enum STATE
        {
            SYSTEM_PARSING,
            SYSTEM_BUILD,
            RUNNING,
            STOPPED,
            STOP_WITH_ERROR
        }
    }
}
