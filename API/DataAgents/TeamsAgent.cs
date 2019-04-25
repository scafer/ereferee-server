using API.Models;
using API.Models.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DataAgents
{
    public class TeamsAgent : AgentBase
    {
        public TeamsAgent()
        {

        }

        public TeamsAgent(AgentBase agent)
              : base(agent) { }


        public Identity CreateTeam(Team team)
        {
            Identity addedTeam = DataContext.ExecuteInsertFromQuery("Team/CreateTeam", team.Name, team.Color);

            if (addedTeam == 0)
            {
                return null;
            }

            return addedTeam;
        }
    }
}
