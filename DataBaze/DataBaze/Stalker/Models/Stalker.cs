using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
//using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace DataBaze.Stalker.Models
{
    public class Stalker
    {
        public int StalkerId { get; set; }
        [JsonIgnore]
        public virtual Avatar Avatar { get; set; } // Связь с моделью аватарки

        [Required(ErrorMessage = "Фамилия является обязательным полем.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Имя является обязательным полем.")]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Никнейм является обязательным полем.")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "PdaId обязателен.")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "PDA ID должен состоять из 3 цифр.")]
        public string PdaId { get; set; }



        [JsonIgnore] // Игнорируем это свойство при сериализации
        public Clan Clan { get; set; } // Связь с классом Clan

        [Required(ErrorMessage = "Клан обязательный.")]
        public int ClanId { get; set; } //Внешний ключ


        public string Location { get; set; }



        [JsonIgnore] // Игнорируем это свойство при сериализации
        public Rank Rank { get; set; } // Связь с классом Rank

        [Required(ErrorMessage = "Ранг обязательный.")]
        public int RankId { get; set; } //Внешний ключ


        // Добавляем коллекцию квестов
        public ICollection<Quest> Quests { get; set; } = new List<Quest>();
    }
}
