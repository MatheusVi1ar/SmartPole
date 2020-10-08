using System;
using System.Collections.Generic;
using System.Text;

namespace SmartPole.Model
{
    public class JSON
    {
        public class Metadata
        {
            public TimeInstant TimeInstant { get; set; }
        }

        public class TimeInstant
        {
            public string type { get; set; }
            public DateTime value { get; set; }
        }
    }
}
