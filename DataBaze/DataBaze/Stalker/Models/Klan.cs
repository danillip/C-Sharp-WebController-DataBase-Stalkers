using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace DataBaze.Stalker.Models
{
    public class Clan
    {
        public int ClanId { get; set; } //PK

        [Required(ErrorMessage = "Название клана обязательно.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Местоположение обязательно.")]
        public string Location { get; set; }

        public int? LeaderStalkerId { get; set; } // Внешний ключ, если нет лидака то NULL

        [JsonIgnore] // Игнорируем это свойство при сериализации
        public Stalker Leader { get; set; } // Связь с классом Stalker, без этого не будет работать .HasOne(c => c.Leader)

        //public ICollection<Stalker> Stalkers { get; set; } = new List<Stalker>(); // Коллекция сталкеров, имеющих данный клан,
        //добавиться еще тогда таблица оно нам не надо

    }
}

//namespace StalkerDb.Models
//{
//    public class Klan
//    {
//        public int ClanId { get; set; }  // PK
//        public string? Name { get; set; }
//        public string? Location { get; set; }

//        // Внешний ключ
//        public int? LeaderStalkerId { get; set; }
//        public Stalker? Leader { get; set; }

//        // Коллекция сталкеров, принадлежащих клану
//        public ICollection<Stalker>? Stalkers { get; set; }  // Связь с сталкерами
//        public ICollection<KlanRelationship>? KlanRelationships1 { get; set; } // Кланы, с которыми этот клан в отношениях
//        public ICollection<KlanRelationship>? KlanRelationships2 { get; set; }
//    }
//}
