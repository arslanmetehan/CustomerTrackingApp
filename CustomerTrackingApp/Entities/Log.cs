using CustomerTrackingApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Entities
{
    public class Log
    {
        public int Id { get; set; }
        public LogType Type { get; set; }
        public string Message { get; set; }
        public long Timestamp { get; set; }
    }
}
