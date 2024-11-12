using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataBaze.Stalker.Models
{
    public class Ammunition
    {
        public int AmmunitionId { get; set; } //PK
        public string Description { get; set; }
        public string Caliber { get; set; }
        public int Type { get; set; }
        public int WeaponId { get; set; } // FK
        public Weapon Weapon { get; set; }  // Маршрут для связи

    }
}
//namespace StalkerDb.Models
//{
//    public class Ammunition
//    {
//        public int AmmunitionId { get; set; }  // PK
//        public string? Description { get; set; }
//        public string? Caliber { get; set; }
//        public string? Type { get; set; }

//        public int WeaponId { get; set; } // FK
//        public Weapon? Weapon { get; set; } // Связь с оружием
//    }
//}
