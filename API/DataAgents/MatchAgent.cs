using API.Models;
using API.Models.Match;
using API.Models.MatchEvents;
using API.Models.Teams;
using Microsoft.AspNetCore.Mvc;
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


        public Identity CreateMatch(Match match, int userId)
        {
            Identity addedMatch = DataContext.ExecuteInsertFromQuery("Match/CreateMatch"
                    , match.Home_Color
                    , match.Visitor_Color
                    , match.Status
                    , userId
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

        public void FinishMatch(int matchId, int homeScore, int visitorScore)
        {
            DataContext.ExecuteCommand("Match/FinishMatch", matchId, homeScore, visitorScore);
        }

        public void DeleteMatch(int matchId)
        {
            DataContext.ExecuteCommand("Match/DeleteMatch", matchId);
        }

        public List<MatchWithTeams> GetPendingMatchs(int userId)
        {
            var matchWithTeamsList = new List<MatchWithTeams>();

            var matchs = DataContext.ExecuteQuery<Match>("Match/GetPendingMatchs", userId).ToArray();

            using (var agent = new TeamsAgent())
            {
                foreach (var match in matchs)
                {
                    var matchwithTeam = new MatchWithTeams();

                    matchwithTeam.Match = match;

                    matchwithTeam.HomeTeam = agent.GetTeam(match.HomeTeamId.Value);
                    matchwithTeam.VisitorTeam = agent.GetTeam(match.VisitorId.Value);

                    matchWithTeamsList.Add(matchwithTeam);
                }

                return matchWithTeamsList;
            }
        }

        public MatchWithTeamsAndMembers GetPendingMatchInfoByID(int matchId)
        {
            using (var agent = new TeamsAgent(this))
            {
                using (var memberAgent = new MembersAgent())
                {
                    var matchWithTeamsAndMembers = new MatchWithTeamsAndMembers();

                    matchWithTeamsAndMembers.Match = DataContext.ExecuteQuery<Match>("Match/GetPendingMatchByID", matchId).FirstOrDefault();

                    matchWithTeamsAndMembers.HomeTeam = agent.GetTeam(matchId);
                    matchWithTeamsAndMembers.VisitorTeam = agent.GetTeam(matchId);

                    matchWithTeamsAndMembers.HomeMembers = DataContext.ExecuteQuery<TeamMember>("Member/GetMembersInfo", matchWithTeamsAndMembers.HomeTeam.TeamId).ToArray();
                    matchWithTeamsAndMembers.VisitorMembers = DataContext.ExecuteQuery<TeamMember>("Member/GetMembersInfo", matchWithTeamsAndMembers.VisitorTeam.TeamId).ToArray();

                    return matchWithTeamsAndMembers;
                }
            }
        }

        public List<MatchWithTeams> GetPreviousMatchs(int userID)
        {
            var matchWithTeamsList = new List<MatchWithTeams>();

            var matchs = DataContext.ExecuteQuery<Match>("Match/GetPreviousMatchs", userID).ToArray();

            using (var agent = new TeamsAgent())
            {
                foreach (var match in matchs)
                {
                    var matchwithTeam = new MatchWithTeams();

                    matchwithTeam.Match = match;

                    matchwithTeam.HomeTeam = agent.GetTeam(match.HomeTeamId.Value);
                    matchwithTeam.VisitorTeam = agent.GetTeam(match.VisitorId.Value);

                    matchWithTeamsList.Add(matchwithTeam);
                }

                return matchWithTeamsList;
            }
        }

        public PreviousMatch GetPreviousMatchByID(int matchID)
        {
            using (var agent = new TeamsAgent(this))
            {
                using (var memberAgent = new MembersAgent())
                {
                    var previousMatch = new PreviousMatch();

                    previousMatch.Match = DataContext.ExecuteQuery<Match>("Match/GetPreviousMatchsByID", matchID).FirstOrDefault();

                    previousMatch.HomeTeam = agent.GetTeam(matchID);
                    previousMatch.VisitorTeam = agent.GetTeam(matchID);

                    previousMatch.HomeMembers = DataContext.ExecuteQuery<TeamMember>("Member/GetMembersInfo", previousMatch.HomeTeam.TeamId).ToList();
                    previousMatch.VisitorMembers = DataContext.ExecuteQuery<TeamMember>("Member/GetMembersInfo", previousMatch.VisitorTeam.TeamId).ToList();

                    // Events
                    previousMatch.Events = DataContext.ExecuteQuery<MatchEvent>("MatchEvent/GetMatchEventsByID", matchID).ToList();

                    return previousMatch;
                }
            }
        }

        public void AssociateUserToMatch(int userId, int matchId, int role)
        {
            DataContext.ExecuteInsertFromQuery("Match/AssociateUserToMatch", userId ,matchId, role);
        }

        public void CreateMatchEvent(MatchEventsType eventsType, int matchId, int? teamId, int? memberId, string description)
        {
            DataContext.ExecuteInsertFromQuery("MatchEvent/CreateMatchEvent", (int)eventsType, matchId, teamId, memberId, description);
        }


    }
}
