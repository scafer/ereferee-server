using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.Teams
{
    public class TeamMember
    {
        public int? TeamId { get; set; }
        public int? MemberID { get; set; }
        public int? Status { get; set; }
        public int Role { get; set; }
        public int Number { get; set; }
        public DateTime? DayStart { get; set; }
        public DateTime? DayEnd { get; set; }
        public string Name { get; set; }
    }
}
