using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaze.Stalker.Models
{
    // Models/User.cs
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // Храните пароли в зашифрованном виде в реальных приложениях
        public string Role { get; set; }
    }

}
