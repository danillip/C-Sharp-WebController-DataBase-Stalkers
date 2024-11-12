using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaze.Stalker.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; } // Заголовок новости
        public string Summary { get; set; } // Краткий текст для отображения в списке
        public string Content { get; set; } // Полный текст новости
        public DateTime CreatedAt { get; set; } // Дата создания
    }
}
