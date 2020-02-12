using ereferee.Models;
using System.Collections.Generic;

namespace ereferee.Services
{
    public class TeamService : BaseService
    {
        DataContext db;

        public TeamService()
        {
            db = new DataContext();
        }

        public Team CreateTeam(Team team)
        {
            db.teams.Add(team);
            db.SaveChanges();
            return team;
        }

        public List<TeamAthlete> CreateAthletes(List<TeamAthlete> athletes, int gameId, int teamId, AthleteType player)
        {
            foreach (TeamAthlete a in athletes)
            {
                a.teamId = teamId;
                var athlete = CreateAthlete(a);

                var teamAthlete = new TeamAthlete { athleteId = athlete.id, teamId = teamId, role = AthleteType.Player.ToString() };
                db.teamAthletes.Add(teamAthlete);
                db.SaveChanges();

                var gameAthlete = new GameAthlete { athleteId = athlete.id, gameId = gameId };
                db.gameAthletes.Add(gameAthlete);
                db.SaveChanges();
            }

            return athletes;
        }

        public Athlete CreateAthlete(TeamAthlete athlete)
        {
            Athlete a = new Athlete { name = athlete.name, teamId = athlete.teamId };
            db.athletes.Add(a);
            db.SaveChanges();
            return a;
        }
    }
}
