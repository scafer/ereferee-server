using API.DataAgents;
using API.Models;
using API.Models.Match;
using API.Models.Members;
using API.Models.Teams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/match/")]
    public class MatchController
    {
        [HttpPost]
        [Route("createMatch")]
        [Authorize]
        public ActionResult<MatchWithTeams> CreateMatch(MatchWithTeams matchWithTeams)
        {
            if (matchWithTeams.Match != null && matchWithTeams.HomeTeam != null && matchWithTeams.VisitorTeam != null
                && matchWithTeams.HomeMembers.Length == 0 && matchWithTeams.VisitorMembers.Length == 0)
            {
                using (var teamAgent = new TeamsAgent())
                {
                    using (var matchAgent = new MatchAgent(teamAgent))
                    {
                        using (var membersAgent = new MembersAgent(matchAgent))
                        {
                            // Create Teams
                            var homeTeamId = teamAgent.CreateTeam(matchWithTeams.HomeTeam);
                            var visitorTeamId = teamAgent.CreateTeam(matchWithTeams.VisitorTeam);

                            if (homeTeamId == null || visitorTeamId == null)
                            {
                                return new NoContentResult();
                            }

                            matchWithTeams.HomeTeam.TeamId = homeTeamId;
                            matchWithTeams.VisitorTeam.TeamId = visitorTeamId;

                            // Create Match
                            var matchId = matchAgent.CreateMatch(matchWithTeams.Match);

                            if (matchId == null)
                            {
                                return new NoContentResult();
                            }

                            matchWithTeams.Match.MatchId = matchId;

                            // Create TeamMembers
                            var homeTeamMembers = membersAgent.CreateTeamMembers(matchWithTeams.HomeMembers, matchWithTeams.Match.MatchId, matchWithTeams.HomeTeam.TeamId, MemberType.Player);
                            var visitorTeamMembers = membersAgent.CreateTeamMembers(matchWithTeams.VisitorMembers, matchWithTeams.Match.MatchId, matchWithTeams.VisitorTeam.TeamId, MemberType.Player);

                            if (homeTeamMembers.Length == 0 || visitorTeamMembers.Length == 0)
                            {
                                return new NoContentResult();
                            }

                            matchWithTeams.HomeMembers = homeTeamMembers;
                            matchWithTeams.VisitorMembers = visitorTeamMembers;

                            return matchWithTeams;
                        }
                    }
                }
            }

            return new NoContentResult();
        }

        [HttpPost]
        [Route("beginMatch")]
        [Authorize]
        public ActionResult<bool> BeginMatch(int matchId)
        {
            if (matchId != 0)
            {
                using (var matchAgent = new MatchAgent())
                {

                    // Validate the MatchId if exists and if is not began already
                    var status = matchAgent.GetMatchStatus(matchId);

                    if (status == 0)
                    {
                        // If not the iniciate the match with the current timne
                        matchAgent.BeginMatch(matchId);

                        return true;
                    }
                }
            }

            return false;
        }

        [HttpPost]
        [Route("getPendingMatchs")]
        [Authorize]
        public ActionResult<Match[]> GetPendingMatchs(User user)
        {
            if (user != null)
            {
                using (var matchAgent = new MatchAgent())
                {
                     return matchAgent.GetPendingMatchs(user.UserID);
                }
            }

            return null;
        }
    }
}
