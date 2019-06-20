using API.Models.MatchEvents;
using API.Models.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.Match
{
    public class PreviousMatch
    {
        public Match Match { get; set; }
        public Team HomeTeam { get; set; }
        public Team VisitorTeam { get; set; }
        public List<TeamMember> HomeMembers { get; set; }
        public List<TeamMember> VisitorMembers { get; set; }
        public List<MatchEvent> Events { get; set; }
    }
}
