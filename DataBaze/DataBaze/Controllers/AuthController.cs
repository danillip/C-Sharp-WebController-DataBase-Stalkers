using Microsoft.AspNetCore.Mvc;
using System.Linq;
using DataBaze.Stalker.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using DataBaze.Services;
using System.ComponentModel.DataAnnotations;
using DataBaze.Stalker.Models;

namespace DataBaze.Controllers
{
    public class AuthController : Controller
    {
        private readonly StalkerContext _context;

        public AuthController(StalkerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            Console.WriteLine("Метод Login (GET) вызван");
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            Console.WriteLine($"Попытка входа с логином: {username}");

            // Поиск пользователя в базе данных
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                // Установка роли пользователя
                HttpContext.Session.SetString("UserRole", user.Role);
                Console.WriteLine($"Успешный вход. Назначена роль: {user.Role}");

                return RedirectToAction("Index", "Home");
            }

            Console.WriteLine("Неверный логин или пароль");
            ModelState.AddModelError("", "Неверный логин или пароль");
            return View();
        }

        [HttpPost]
        public IActionResult GuestLogin()
        {
            Console.WriteLine("Метод GuestLogin вызван");

            HttpContext.Session.SetString("UserRole", "Guest");
            Console.WriteLine("Кнопка 'Войти как Гость' нажата. Роль установлена: Гость");

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            Console.WriteLine("Метод Logout вызван");
            Console.WriteLine($"Текущая роль перед выходом: {HttpContext.Session.GetString("UserRole")}");

            HttpContext.Session.Clear();
            Console.WriteLine("Выход из системы. Сессия очищена.");

            return RedirectToAction("Index", "Home");
        }
    }
}
