using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.MatchEvents
{
    public enum MatchEventsType
    {
        First_Half = 1,
        Second_Half = 2,
        OverTime = 3,
        Penalties = 4,
        Goal = 5,
        Penalty_Goal = 6,
        Yellow_Card = 7,
        Red_Card = 8,
        Substitution = 9,
        Random_Notes = 10
    }
}
