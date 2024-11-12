using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaze.Lessons
{
    public class Coach
    {
        public int CoachId { get; set; }
        public required string CoachName { get; set; }
        public int Age { get; set; }

        public int? TeamId { get; set; }
        public Team? Team { get; set; }  // Nullable свойство

        public Coach()  // Пустой конструктор
        {
        }
    }
}
