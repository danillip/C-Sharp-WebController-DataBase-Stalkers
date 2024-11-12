using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaze.Lessons
{
    public class Team
    {
        public int TeamId { get; set; }
        public required string TeamName { get; set; }  // Используем модификатор required

        public ICollection<Player> Players { get; set; }
        public ICollection<Coach> Coaches { get; set; }

        public Team()
        {
            Players = new List<Player>();
            Coaches = new List<Coach>();
        }

    }
}
