using System.Collections.Generic;

namespace ereferee.Models
{
    public class GameData
    {
        public Game game { get; set; }
        public Team? homeTeam { get; set; }
        public Team? visitorTeam { get; set; }
        public List<Athlete>? homeAthletes { get; set; }
        public List<Athlete>? visitorAthletes { get; set; }
        public List<Event>? events { get; set; }
    }
}
