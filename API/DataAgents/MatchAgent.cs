using API.Models;
using API.Models.Match;
using API.Models.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DataAgents
{
    public class MatchAgent : AgentBase
    {
        public MatchAgent()
        {

        }

        public MatchAgent(AgentBase agent)
                : base(agent) { }


        public Identity CreateMatch(Match match)
        {
            Identity addedMatch = DataContext.ExecuteInsertFromQuery("Match/CreateMatch", 
                      match.TimeStart
                    , match.Home_Color
                    , match.Home_Score
                    , match.Status
                    , match.MatchOwnerId
                    , match.HomeTeamId
                    , match.VisitorId);

            if (addedMatch == 0)
            {
                return null;
            }

            return addedMatch;
        }

    }
}
