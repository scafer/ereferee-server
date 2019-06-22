﻿using API.Models;
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

        public MatchWithEvents GetPendingMatchInfoByID(int matchId)
        {
            using (var agent = new TeamsAgent(this))
            {
                using (var memberAgent = new MembersAgent())
                {
                    var matchWithEvents = new MatchWithEvents();

                    matchWithEvents.Match = DataContext.ExecuteQuery<Match>("Match/GetPendingMatchByID", matchId).FirstOrDefault();

                    matchWithEvents.HomeTeam = agent.GetTeam(matchWithEvents.Match.HomeTeamId.Value);
                    matchWithEvents.VisitorTeam = agent.GetTeam(matchWithEvents.Match.VisitorId.Value);

                    matchWithEvents.HomeMembers = DataContext.ExecuteQuery<TeamMember>("Member/GetMembersInfo", matchWithEvents.HomeTeam.TeamId).ToList();
                    matchWithEvents.VisitorMembers = DataContext.ExecuteQuery<TeamMember>("Member/GetMembersInfo", matchWithEvents.VisitorTeam.TeamId).ToList();

                    matchWithEvents.Events = new List<MatchEvent>();

                    return matchWithEvents;
                }
            }
        }

        public List<MatchWithTeams> GetActiveMatchs(int userID)
        {
            var matchWithTeamsList = new List<MatchWithTeams>();

            var matchs = DataContext.ExecuteQuery<Match>("Match/GetActiveMatchs", userID).ToArray();

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

        public MatchWithEvents GetActiveMatchByID(int matchID)
        {
            using (var agent = new TeamsAgent(this))
            {
                using (var memberAgent = new MembersAgent())
                {
                    var previousMatch = new MatchWithEvents();

                    previousMatch.Match = DataContext.ExecuteQuery<Match>("Match/GetActiveMatchsByID", matchID).FirstOrDefault();

                    previousMatch.HomeTeam = agent.GetTeam(previousMatch.Match.HomeTeamId.Value);
                    previousMatch.VisitorTeam = agent.GetTeam(previousMatch.Match.VisitorId.Value);

                    previousMatch.HomeMembers = DataContext.ExecuteQuery<TeamMember>("Member/GetMembersInfo", previousMatch.HomeTeam.TeamId).ToList();
                    previousMatch.VisitorMembers = DataContext.ExecuteQuery<TeamMember>("Member/GetMembersInfo", previousMatch.VisitorTeam.TeamId).ToList();

                    // Events
                    previousMatch.Events = DataContext.ExecuteQuery<MatchEvent>("MatchEvent/GetMatchEventsByID", matchID).ToList();

                    return previousMatch;
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

        public MatchWithEvents GetPreviousMatchByID(int matchID)
        {
            using (var agent = new TeamsAgent(this))
            {
                using (var memberAgent = new MembersAgent())
                {
                    var previousMatch = new MatchWithEvents();

                    previousMatch.Match = DataContext.ExecuteQuery<Match>("Match/GetPreviousMatchsByID", matchID).FirstOrDefault();

                    previousMatch.HomeTeam = agent.GetTeam(previousMatch.Match.HomeTeamId.Value);
                    previousMatch.VisitorTeam = agent.GetTeam(previousMatch.Match.VisitorId.Value);

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

        public void CreateMatchEvent(int userID, MatchEventsType eventsType, int matchId, int? memberId, string description, string matchTime)
        {
            DataContext.ExecuteInsertFromQuery("MatchEvent/CreateMatchEvent", userID, (int)eventsType, matchId, memberId, description, matchTime);
        }
    }
}
