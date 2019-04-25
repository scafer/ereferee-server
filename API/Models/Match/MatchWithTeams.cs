using API.Models.Members;
using API.Models.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.Match
{
    public class MatchWithTeams
    {
        public Match Match { get; set; }
        public Team HomeTeam { get; set; }
        public Team VisitorTeam { get; set; }
        public TeamMember[] HomeMembers { get; set; }
        public TeamMember[] VisitorMembers { get; set; }
    }
}
