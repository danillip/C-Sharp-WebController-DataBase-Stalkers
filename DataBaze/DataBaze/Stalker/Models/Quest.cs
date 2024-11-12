using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaze.Stalker.Models
{
    public class Quest
    {
        public int QuestId { get; set; } //PK
        public int StalkerId { get; set; } // FK
        public Stalker Stalker { get; set; } //Разкоментить после реализации пушек
        public int ArtifactId { get; set; } // FK
        public Artifact Artifact { get; set; }
        public int RewardWeaponId { get; set; } // FK
        public Weapon RewardWeapon { get; set; }
        public string Description { get; set; }

    }
}

//namespace StalkerDb.Models
//{
//    public class Quest
//    {
//        public int QuestId { get; set; }  // PK
//        public int StalkerId { get; set; } // FK
//        public int ArtifactId { get; set; } // FK
//        public int RewardWeaponId { get; set; } // FK
//        public string? Description { get; set; }

//        public Stalker? Stalker { get; set; }
//        public Artifact? Artifact { get; set; }
//        public Weapon? RewardWeapon { get; set; }
//    }
//}
