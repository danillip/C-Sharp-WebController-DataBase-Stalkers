using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaze.Stalker.Models
{
    public class KlanRelationship
    {
        public int KlanRelationshipId { get; set; } //PK

        public int ClanId1 { get; set; } //FK
        public Clan Clan1 { get; set; }  // Навигационное свойство для клана 1


        public int ClanId2 { get; set; } //FK
        public Clan Clan2 { get; set; }  // Навигационное свойство для клана 2

        public string? RelationshipType { get; set; }
        public string? Description { get; set; }

    }
}

//namespace StalkerDb.Models
//{
//    public class KlanRelationship
//    {
//        public int KlanRelationshipId { get; set; } // PK
//        public int ClanId1 { get; set; } // FK
//        public int ClanId2 { get; set; } // FK
//        public string? RelationshipType { get; set; }
//        public string? Description { get; set; }

//        public Klan? Klan1 { get; set; }
//        public Klan? Klan2 { get; set; }
//    }
//}
