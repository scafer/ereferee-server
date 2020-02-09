using System.Collections.Generic;

#nullable enable
namespace ereferee.Models
{
    public class GameData
    {
        public Game? game { get; set; }
        public Team? homeTeam { get; set; }
        public Team? visitorTeam { get; set; }
        public List<TeamAthlete>? homeAthletes { get; set; }
        public List<TeamAthlete>? visitorAthletes { get; set; }
        public List<GameEvent>? events { get; set; }
    }
}
