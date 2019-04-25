using API.DataAgents;
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
    [Route("api/Auth/")]
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
                MatchWithTeams matchWithTeamMembers = new MatchWithTeams();

                using (var agent = new TeamsAgent())
                {
                    var homeTeamId = agent.CreateTeam(matchWithTeams.HomeTeam);

                    var visitorTeamId = agent.CreateTeam(matchWithTeams.VisitorTeam);

                    if (homeTeamId == null || visitorTeamId == null)
                    {
                        return new NoContentResult();
                    }

                    matchWithTeamMembers.Match = matchWithTeams.Match;
                    matchWithTeamMembers.HomeTeam = matchWithTeams.HomeTeam;
                    matchWithTeamMembers.HomeTeam.TeamId = homeTeamId;
                    matchWithTeamMembers.VisitorTeam = matchWithTeams.VisitorTeam;
                    matchWithTeamMembers.VisitorTeam.TeamId = visitorTeamId;
                }

                using (var agent = new MatchAgent())
                {
                    var matchId = agent.CreateMatch(matchWithTeams.Match);

                    if (matchId == null)
                    {
                        return new NoContentResult();
                    }

                    matchWithTeamMembers.Match.MatchId = matchId;
                }

                using (var agent = new MembersAgent())
                {
                    var homeTeamMembers = agent.CreateTeamMembers(matchWithTeams.HomeMembers, matchWithTeamMembers.Match.MatchId, matchWithTeamMembers.HomeTeam.TeamId);
                    var visitorTeamMembers = agent.CreateTeamMembers(matchWithTeams.VisitorMembers, matchWithTeamMembers.Match.MatchId, matchWithTeamMembers.VisitorTeam.TeamId);

                    if (homeTeamMembers.Length == 0 || visitorTeamMembers.Length == 0 )
                    {
                        return new NoContentResult();
                    }

                    matchWithTeamMembers.HomeMembers = homeTeamMembers;
                    matchWithTeamMembers.VisitorMembers = visitorTeamMembers;
                }

                return matchWithTeamMembers;
            }
            return new NoContentResult();
        }
    }
}
