using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace DataBaze.Stalker.Models
{
    public class Rank
    {
        public int RankId { get; set; } //PK
        public string RankName { get; set; }
        public string MutantsKilledRequired { get; set; }

        //public ICollection<Stalker> Stalkers { get; set; } = new List<Stalker>(); // Коллекция сталкеров, имеющих данный ранг
    }
}

//{
//    public class Rank
//    {
//        public int RankId { get; set; } // PK
//        public string? RankName { get; set; }
//        public int MutantsKilledRequired { get; set; }

//        public ICollection<Stalker>? Stalkers { get; set; } // Связь с сталкерами
//    }
//}
