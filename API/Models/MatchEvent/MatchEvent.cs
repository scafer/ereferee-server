using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.MatchEvents
{
    public class MatchEvent
    {
        public int? MatchID { get; set; }
        public int? UserID { get; set; }
        public int? MemberID { get; set; }
        public MatchEventsType EventType { get; set; }
        public DateTime DateTime { get; set; }
        public string Event_Description { get; set; }
        public string Reg { get; set; }

    }
}
