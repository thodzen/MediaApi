using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MediaApi.Helpers
{
    public class SystemTime : ISystemTime
    {
        public DateTime GetCurrent()
        {
            return DateTime.Now;
        }
    }
}
