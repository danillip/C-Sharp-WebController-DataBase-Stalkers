using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataBaze.Stalker.Models
{
    public class Avatar
    {
        public int Id { get; set; }              // Уникальный идентификатор аватарки
        public string ImagePath { get; set; }    // Путь к файлу аватарки
        public int StalkerId { get; set; }       // Идентификатор сталкера, к которому относится аватарка
        public Stalker Stalker { get; set; }     // Навигационное свойство для связи со сталкером
    }

}
