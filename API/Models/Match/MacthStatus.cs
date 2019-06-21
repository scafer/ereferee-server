using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.Match
{
    public enum MacthStatus
    {
        Deleted = -1,
        Pending = 0,
        Active = 1,
        Finished = 2
    }
}
