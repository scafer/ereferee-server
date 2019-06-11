using API.Models;
using API.Models.Members;
using API.Models.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DataAgents
{
    public class MembersAgent : AgentBase
    {
        public MembersAgent()
        {

        }

        public MembersAgent(AgentBase agent)
              : base(agent) { }


        public TeamMember[] CreateTeamMembers(TeamMember[] members, int? matchId, int? teamId, MemberType memberType)
        {
            List<TeamMember> teamMembers = new List<TeamMember>();

            using (var t = CreateTransactionScope())
            {
                var valid = true;

                foreach (var member in members)
                {
                    // Add member 
                    var memberId = CreateMember(member.Name);
                    // Add team Member 
                    var teamMemberId = CreateTeamMember(teamId.Value, memberId, member);
                    // Add match Member
                    var matchMember = CreateMatchMember(matchId.Value, memberId, memberType.ToString());

                    if (memberId.Id == 0 || teamMemberId.Id == 0|| matchMember.Id == 0)
                    {
                        valid = false;
                    }
                    else
                    {
                        member.MemberID = memberId;
                        teamMembers.Add(member);
                    }
                }

                if (valid)
                {
                    t.Complete();
                }
            }

            return teamMembers.ToArray();
        }

        private Identity CreateMatchMember(int matchId, int memberId, string reg)
        {
            Identity matchMember = DataContext.ExecuteInsertFromQuery("Match/CreateMatchMember", matchId, memberId, reg);

            if (matchMember == 0)
            {
                return null;
            }

            return matchMember;
        }

        public Identity CreateMember(string name)
        {
            Identity addedMember = DataContext.ExecuteInsertFromQuery("Member/CreateMember", name);

            if (addedMember == 0)
            {
                return null;
            }

            return addedMember;
        }

        public Identity CreateTeamMember(int matchId, int teamId, TeamMember teamMember)
        {
            Identity addedMember = DataContext.ExecuteInsertFromQuery("Team/CreateTeamMember", matchId, teamId, teamMember.Status, teamMember.Role, teamMember.Number);

            if (addedMember == 0)
            {
                return null;
            }

            return addedMember;
        }
    }
}
