using System;
using System.Collections.Generic;
using System.Text;

namespace Task1App.Models
{
    public class Client
    {
        public string Name { get; set; }
        public string TortureDescription { get; set; }
        public int SessionCount { get; set; }
        public SessionStatus SessionStatus { get; set; } = SessionStatus.Pending;
        public DateTime? StartTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
