using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaze.Stalker.Models
{
    public class Weapon
    {
        public int WeaponId { get; set; } //PK
        public string Name { get; set; }
        public string Type { get; set; }
        public int FireRate { get; set; }
        public int Range { get; set; }
        public int MagazineSize { get; set; }
        public string Description { get; set; }

        public ICollection<Ammunition>? Ammunitions { get; set; }

    }
}

//namespace StalkerDb.Models
//{
//    public class Weapon
//    {
//        public int WeaponId { get; set; }  // PK
//        public string? Name { get; set; }
//        public string? Type { get; set; }
//        public int FireRate { get; set; }
//        public int Range { get; set; }
//        public int MagazineSize { get; set; }
//        public string? Description { get; set; }

//        public ICollection<Ammunition>? Ammunitions { get; set; } // Связь с патронами
//    }
//}
