using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.Match
{
    public class Match
    {
        public int? MatchId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public int? Home_Score { get; set; }
        public string Home_Color { get; set; }
        public int? Visitor_Score { get; set; }
        public string Visitor_Color { get; set; }
        public int? Status { get; set; }
        public DateTime? Dt_Creation { get; set; }
        public int? MatchOwnerId { get; set; }
        public int? HomeTeamId { get; set; }
        public int? VisitorId { get; set; }
    }
}
