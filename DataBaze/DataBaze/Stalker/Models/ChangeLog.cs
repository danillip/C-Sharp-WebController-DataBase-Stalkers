using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataBaze.Stalker.Models
{
    public class ChangeLog
    {
        public int Id { get; set; }

        [Required] // Обязательное поле
        [StringLength(50)] // Максимальная длина строки
        public string Action { get; set; } // Добавление, Изменение, Удаление

        [Required] // Обязательное поле
        [StringLength(50)] // Максимальная длина строки
        public string EntityType { get; set; } // Тип сущности (Сталкер, Клан, Артефакт)

        [Required] // Обязательное поле
        [StringLength(100)] // Максимальная длина строки
        public string EntityName { get; set; } // Имя или ID сущности

        [Required] // Обязательное поле
        [StringLength(100)] // Максимальная длина строки
        public string ChangedBy { get; set; } // Кто сделал изменение

        public DateTime ChangeTime { get; set; } // Время изменения
    }
}
