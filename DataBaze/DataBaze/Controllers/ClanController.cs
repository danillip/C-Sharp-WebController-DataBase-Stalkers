using Microsoft.AspNetCore.Mvc;
using DataBaze.Stalker.Models;
using System.Linq;
using DataBaze.Stalker.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using DataBaze.Services;
using System.ComponentModel.DataAnnotations;


namespace DataBaze.Stalker.Controllers
{
    public class ClanController : Controller
    {
        private readonly StalkerContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ChangeLogService _changeLogService;

        public ClanController(StalkerContext context, IHttpContextAccessor httpContextAccessor, ChangeLogService changeLogService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); // Защита от null
            _httpContextAccessor = httpContextAccessor;
            _changeLogService = changeLogService;
        }

        // GET: Clan
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5; // Количество записей на одной странице
            var clans = await _context.Clans
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)_context.Clans.Count() / pageSize);
            ViewBag.NonLeaderStalkers = await _context.Stalkers
                .Where(s => !_context.Clans.Select(c => c.LeaderStalkerId).Contains(s.StalkerId))
                .ToListAsync();

            // Получение роли пользователя
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Session.GetString("UserRole");

            return View(clans);
        }

        // POST: Clan/Add
        [HttpPost]
        public async Task<IActionResult> Add(ClanDto clanDto)
        {
            Console.WriteLine($"Received data - Name: {clanDto.Name}, Location: {clanDto.Location}, LeaderStalkerId: {clanDto.LeaderStalkerId}");

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(clanDto.LeaderStalkerId?.ToString()))
                {
                    clanDto.LeaderStalkerId = null;
                }
                else
                {
                    var leaderStalker = await _context.Stalkers.FindAsync(clanDto.LeaderStalkerId);
                    if (leaderStalker == null)
                    {
                        ModelState.AddModelError("LeaderStalkerId", "Сталкер с указанным ID не найден.");
                    }
                    else if (_context.Clans.Any(c => c.LeaderStalkerId == clanDto.LeaderStalkerId))
                    {
                        ModelState.AddModelError("LeaderStalkerId", "Этот сталкер уже является лидером другого клана.");
                    }
                }

                if (ModelState.IsValid)
                {
                    var clan = new Clan
                    {
                        Name = clanDto.Name,
                        Location = clanDto.Location,
                        LeaderStalkerId = clanDto.LeaderStalkerId
                    };

                    _context.Clans.Add(clan);
                    await _context.SaveChangesAsync();

                    //------------------------------------------------------LOGS
                    // Логируем добавление нового клана
                    var changeLog = new ChangeLog
                    {
                        Action = "Добавление",
                        EntityType = "Клан",
                        EntityName = clan.Name,
                        ChangedBy = _httpContextAccessor.HttpContext.Session.GetString("UserRole"),
                        ChangeTime = DateTime.Now
                    };
                    await _changeLogService.LogChangeAsync(changeLog);
                    //------------------------------------------------------LOGS

                    return Json(new { success = true });
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            Console.WriteLine("ModelState Errors: " + string.Join(", ", errors));

            return Json(new { success = false, errors });
        }



        // POST: Clan/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(ClanDto clanDto)
        {
            Console.WriteLine($"Редактируем клан: Name='{clanDto.Name}', Location='{clanDto.Location}', LeaderStalkerId='{clanDto.LeaderStalkerId}'");

            if (ModelState.IsValid)
            {
                var existingClan = await _context.Clans.FindAsync(clanDto.ClanId);
                if (existingClan != null)
                {
                    existingClan.Name = clanDto.Name;
                    existingClan.Location = clanDto.Location;
                    existingClan.LeaderStalkerId = clanDto.LeaderStalkerId;

                    try
                    {
                        await _context.SaveChangesAsync();

                        //------------------------------------------------------LOGS
                        // Логируем редактирование клана
                        var changeLog = new ChangeLog
                        {
                            Action = "Редактирование",
                            EntityType = "Клан",
                            EntityName = existingClan.Name,
                            ChangedBy = _httpContextAccessor.HttpContext.Session.GetString("UserRole"),
                            ChangeTime = DateTime.Now
                        };
                        await _changeLogService.LogChangeAsync(changeLog);
                        //------------------------------------------------------LOGS

                        return Json(new { success = true });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при сохранении: {ex.Message}");
                        return Json(new { success = false, message = "Ошибка при сохранении изменений." });
                    }
                }
            }

            // Логируем ошибки валидации
            Console.WriteLine("Ошибка в модели при редактировании клана:");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Ошибка: {error.ErrorMessage}");
            }

            return Json(new { success = false });
        }



        // POST: Clan/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var clan = await _context.Clans.FindAsync(id);
            if (clan != null)
            {
                _context.Clans.Remove(clan);
                await _context.SaveChangesAsync();

                //------------------------------------------------------LOGS
                // Логируем удаление клана
                var changeLog = new ChangeLog
                {
                    Action = "Удаление",
                    EntityType = "Клан",
                    EntityName = clan.Name,
                    ChangedBy = _httpContextAccessor.HttpContext.Session.GetString("UserRole"),
                    ChangeTime = DateTime.Now
                };
                await _changeLogService.LogChangeAsync(changeLog);
                //------------------------------------------------------LOGS

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

    }

    // DTO для клана
    public class ClanDto
    {
        public int ClanId { get; set; }
        [Required(ErrorMessage = "Название клана обязательно.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Расположение клана обязательно.")]
        public string Location { get; set; }
        public int? LeaderStalkerId { get; set; } // Внешний ключ, если нет лидака, то NULL
    }
}
