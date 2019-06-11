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
            Identity addedMatch = DataContext.ExecuteInsertFromQuery("Match/CreateMatch"
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

        public int GetMatchStatus(int matchId)
        {
            return (DataContext.ExecuteQuery<int>("Match/GetMatchStatus", matchId).FirstOrDefault());
        }

        public void BeginMatch(int matchId)
        {
            DataContext.ExecuteCommand("Match/BeginMatch", matchId);
        }

        public Match[] GetPendingMatchs(int userID)
        {
            return (DataContext.ExecuteQuery<Match>("Match/GetPendingMatchs", userID).ToArray());
        }
    }
}
