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
                       
            foreach (var member in members)
            {
                // Add member 
                var memberId = CreateMember(member.Name);
                // Add team Member 
                CreateTeamMember(teamId.Value, memberId, member);
                // Add match Member
                CreateMatchMember(matchId.Value, memberId, (int)memberType);

                if (memberId.Id == 0)
                {
                    return null;
                }
                else
                {
                    member.MemberID = memberId;
                    teamMembers.Add(member);
                }
            }

            return teamMembers.ToArray();
        }

        private void CreateMatchMember(int matchId, int memberId, int memberType)
        {
            DataContext.ExecuteInsertFromQuery("Match/CreateMatchMember", matchId, memberId, memberType);
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

        public void CreateTeamMember(int teamId, int memberId, TeamMember teamMember)
        {
           DataContext.ExecuteInsertFromQuery("Team/CreateTeamMember", teamId, memberId, teamMember.Status, teamMember.Role, teamMember.Number);
        }

        //public TeamMember[] GetTeamMembersByMatchId(int matchId)
        //{

        //}
    }
}
