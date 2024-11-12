using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DataBaze.Lessons
{
    public class Player
    {
        public int PlayerId { get; set; }
        public required string PlayerName { get; set; }  // Устанавливаем свойство как required
        public required string Position { get; set; }    // Также required
        public int Age { get; set; }

        public ICollection<Team> Teams { get; set; }

        public Player()
        {
            Teams = new List<Team>();
        }
        public void PrintPlayer()
        {
            Console.WriteLine(PlayerName);
        }
    }
}
