using API.DataAgents;
using API.Extensions;
using API.Models;
using API.Models.Match;
using API.Models.MatchEvents;
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
    public class MatchController : ControllerBase
    {
        [HttpPost]
        [Route("createMatch")]
        [Authorize]
        [DisableRequestSizeLimit]
        public ActionResult<MatchWithTeamsAndMembers> CreateMatch([FromBody]MatchWithTeamsAndMembers matchWithTeams)
        {
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }

            if (matchWithTeams.Match != null && matchWithTeams.HomeTeam != null && matchWithTeams.VisitorTeam != null
                && matchWithTeams.HomeMembers.Length != 0 && matchWithTeams.VisitorMembers.Length != 0)
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

                            matchWithTeams.HomeTeam.TeamId = matchWithTeams.Match.HomeTeamId = homeTeamId;
                            matchWithTeams.VisitorTeam.TeamId = matchWithTeams.Match.VisitorId = visitorTeamId;

                            // Create Match
                            var matchId = matchAgent.CreateMatch(matchWithTeams.Match, user.UserID);

                            if (matchId == null)
                            {
                                return new NoContentResult();
                            }

                            matchWithTeams.Match.MatchId = matchId;

                            // Create TeamMembers
                            var homeTeamMembers = membersAgent.CreateTeamMembers(matchWithTeams.HomeMembers, matchWithTeams.Match.MatchId, matchWithTeams.HomeTeam.TeamId, MemberType.Player);
                            var visitorTeamMembers = membersAgent.CreateTeamMembers(matchWithTeams.VisitorMembers, matchWithTeams.Match.MatchId, matchWithTeams.VisitorTeam.TeamId, MemberType.Player);

                            if (homeTeamMembers == null || visitorTeamMembers == null)
                            {
                                return new NoContentResult();
                            }

                            matchWithTeams.Match.MatchOwnerId = user.UserID;
                            matchWithTeams.HomeMembers = homeTeamMembers;
                            matchWithTeams.VisitorMembers = visitorTeamMembers;

                            matchAgent.AssociateUserToMatch(user.UserID, matchId, 1);

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
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }

            if (matchId != 0)
            {
                using (var matchAgent = new MatchAgent())
                {
                    // Validate the MatchId if exists and if is not began already
                    var status = matchAgent.GetMatchStatus(matchId);

                    if (status == 0)
                    {
                        matchAgent.BeginMatch(matchId);

                        return true;
                    }
                }
            }

            return false;
        }

        [HttpPost]
        [Route("finishMatch")]
        [Authorize]
        public ActionResult<bool> FinishMatch(int matchId, int homeScore, int visitorScore)
        {
            var user = User.GetUser();

            if (user == null)
            {
                return new NotFoundResult();
            }

            if (matchId != 0)
            {
                using (var matchAgent = new MatchAgent())
                {
                    var status = matchAgent.GetMatchStatus(matchId);

                    if (status == 1)
                    {
                        matchAgent.FinishMatch(matchId, homeScore, visitorScore);

                        return true;
                    }
                }
            }

            return false;
        }

        [HttpGet]
        [Route("getPendingMatchs")]
        [Authorize]
        public ActionResult<List<MatchWithTeams>> GetPendingMatchs()
        {
            var user = User.GetUser();

            if (user != null)
            {
                using (var matchAgent = new MatchAgent())
                {
                     return matchAgent.GetPendingMatchs(user.UserID);
                }
            }

            return new NotFoundResult();
        }

        [HttpGet]
        [Route("getPendingMatchByID")]
        [Authorize]
        public ActionResult<MatchWithEvents> GetPendingMatchByID(int matchID)
        {
            var user = User.GetUser();

            if (user != null)
            {
                using (var matchAgent = new MatchAgent())
                {
                    return matchAgent.GetPendingMatchInfoByID(matchID);
                }
            }

            return new NotFoundResult();
        }

        [HttpGet]
        [Route("getActiveMatchs")]
        [Authorize]
        public ActionResult<List<MatchWithTeams>> GetActiveMatchs()
        {
            var user = User.GetUser();

            if (user != null)
            {
                using (var matchAgent = new MatchAgent())
                {
                    return matchAgent.GetActiveMatchs(user.UserID);
                }
            }

            return new NotFoundResult();
        }

        [HttpGet]
        [Route("getActiveMatchByID")]
        [Authorize]
        public ActionResult<MatchWithEvents> GetActiveMatchByID(int matchID)
        {
            var user = User.GetUser();

            if (user != null)
            {
                using (var matchAgent = new MatchAgent())
                {
                    return matchAgent.GetActiveMatchByID(matchID);
                }
            }

            return new NotFoundResult();
        }


        [HttpGet]
        [Route("getPreviousMatchs")]
        [Authorize]
        public ActionResult<List<MatchWithTeams>> GetPreviousMatchs()
        {
            var user = User.GetUser();

            if (user != null)
            {
                using (var matchAgent = new MatchAgent())
                {
                    return matchAgent.GetPreviousMatchs(user.UserID);
                }
            }

            return new NotFoundResult();
        }

        [HttpGet]
        [Route("getPreviousMatchByID")]
        [Authorize]
        public ActionResult<MatchWithEvents> GetPreviousMatchByID(int matchID)
        {
            var user = User.GetUser();

            if (user != null)
            {
                using (var matchAgent = new MatchAgent())
                {
                    return matchAgent.GetPreviousMatchByID(matchID);
                }
            }

            return new NotFoundResult();
        }


        [HttpPost]
        [Route("createMatchEvents")]
        [Authorize]
        public ActionResult<bool> CreateMatchEvents(MatchEventsType eventType, int matchID, int? memberId, string description, string matchTime)
        {
            var user = User.GetUser();

            if (user != null)
            {
                using (var matchAgent = new MatchAgent())
                {
                    matchAgent.CreateMatchEvent(user.UserID, eventType, matchID, memberId, description, matchTime);

                    return true;
                }
            }

            return new NotFoundResult();
        }

        [HttpPost]
        [Route("deleteMatch")]
        [Authorize]
        public ActionResult<bool> DeleteMatch(int matchID)
        {
            var user = User.GetUser();

            if (user != null)
            {
                using (var matchAgent = new MatchAgent())
                {
                    matchAgent.DeleteMatch(matchID);

                    return true;
                }
            }

            return new NotFoundResult();
        }
    }
}
