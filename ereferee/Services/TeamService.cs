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

        public List<Athlete> CreateAthletes(List<Athlete> athletes, int gameId, int teamId, AthleteType player)
        {
            foreach (Athlete a in athletes)
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

        public Athlete CreateAthlete(Athlete athlete)
        {
            db.athletes.Add(athlete);
            db.SaveChanges();
            return athlete;
        }
    }
}
