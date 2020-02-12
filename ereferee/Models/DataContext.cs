using ereferee.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ereferee.Models
{
    public class DataContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Game> games { get; set; }
        public DbSet<Team> teams { get; set; }
        public DbSet<Athlete> athletes { get; set; }
        public DbSet<Event> events { get; set; }
        public DbSet<TeamAthlete> teamAthletes { get; set; }
        public DbSet<UserGame> userGames { get; set; }
        public DbSet<GameEvent> gameEvents { get; set; }
        public DbSet<GameAthlete> gameAthletes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configurations.GetNpgsqlConnectionString());
        }
    }

    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
    }

    public class Game
    {
        public int id { get; set; }
        public string timeStart { get; set; }
        public string timeEnd { get; set; }
        public int homeScore { get; set; }
        public int visitorScore { get; set; }
        public string homeColor { get; set; }
        public string visitorColor { get; set; }
        public int status { get; set; }
        public string creationDate { get; set; }
        public int userId { get; set; }
        public int homeTeamId { get; set; }
        public int visitorTeamId { get; set; }
    }

    public class Team
    {
        public int id { get; set; }
        public string name { get; set; }
        public string color { get; set; }
    }

    public class Athlete
    {
        public int id { get; set; }
        public string name { get; set; }
        public int teamId { get; set; }
    }

    public class Event
    {
        public int id { get; set; }
        public string description { get; set; }
    }

    public class TeamAthlete
    {
        public int id { get; set; }
        public string name { get; set; }
        public int teamId { get; set; }
        public int athleteId { get; set; }
        public string status { get; set; }
        public string role { get; set; }
        public int number { get; set; }
        public string dayStart { get; set; }
        public string dayEnd { get; set; }
    }

    public class UserGame
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int gameId { get; set; }
        public int role { get; set; }
    }

    public class GameEvent
    {
        public int id { get; set; }
        public int gameId { get; set; }
        public int userId { get; set; }
        public int athleteId { get; set; }
        public string time { get; set; }
        public string eventDescription { get; set; }
        public string reg { get; set; }
    }

    public class GameAthlete
    {
        public int id { get; set; }
        public int gameId { get; set; }
        public int athleteId { get; set; }
        public string reg { get; set; }
    }
}