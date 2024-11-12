using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataBaze.Stalker.Models;
using System.Threading.Tasks;
using DataBaze.Stalker.Data;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using DataBaze.Services;

namespace DataBaze.Controllers
{
    public class StalkerController : Controller
    {
        private readonly StalkerContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ChangeLogService _changeLogService;

        public StalkerController(StalkerContext context, IHttpContextAccessor httpContextAccessor, ChangeLogService changeLogService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _changeLogService = changeLogService;
        }

        // GET: Stalkers
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5; // Количество записей на одной странице
            var stalkers = await _context.Stalkers
                .Include(s => s.Clan)
                .Include(s => s.Avatar)  // Включаем аватар в запрос
                .Include(s => s.Rank)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)_context.Stalkers.Count() / pageSize);
            ViewBag.Clans = await _context.Clans.ToListAsync();
            ViewBag.Ranks = await _context.Ranks.ToListAsync();
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Session.GetString("UserRole");

            // Отладочная информация
            foreach (var stalker in stalkers)
            {
                Console.WriteLine($"Stalker {stalker.StalkerId} has avatar path: {stalker.Avatar?.ImagePath ?? "No avatar"}");
            }

            return View(stalkers);
        }

        // POST: Stalkers/CheckPdaIdUnique
        [HttpGet]
        public async Task<JsonResult> CheckPdaIdUnique(string pdaId)
        {
            // Добавляем префикс "PDA" к введенному PdaId
            string fullPdaId = "PDA" + pdaId;

            // Проверка на уникальность PdaId
            bool isUnique = !await _context.Stalkers.AnyAsync(s => s.PdaId == fullPdaId);
            return Json(new { isUnique });
        }




        // POST: Stalkers/Create
        [HttpPost]
        public async Task<JsonResult> Create(StalkerDto stalkerDto) // Изменено на StalkerDto
        {
            Console.WriteLine("Полученные данные: " + JsonConvert.SerializeObject(stalkerDto));

            // Убедится, что ClanId и RankId присутствуют
            if (stalkerDto.ClanId == 0 || stalkerDto.RankId == 0)
            {
                return Json(new { success = false, errors = new[] { "ClanId и RankId должны быть указаны." } });
            }

            // Создаем новый объект Stalker на основе данных из DTO
            var stalker = new DataBaze.Stalker.Models.Stalker
            {
                LastName = stalkerDto.LastName,
                FirstName = stalkerDto.FirstName,
                MiddleName = string.IsNullOrEmpty(stalkerDto.MiddleName) ? null : stalkerDto.MiddleName,
                Nickname = stalkerDto.Nickname,
                PdaId = stalkerDto.PdaId,
                ClanId = stalkerDto.ClanId,
                Location = string.IsNullOrEmpty(stalkerDto.Location) ? null : stalkerDto.Location,
                RankId = stalkerDto.RankId
            };

            // Проверка уникальности PdaId
            if (await _context.Stalkers.AnyAsync(s => s.PdaId == stalker.PdaId))
            {
                return Json(new { success = false, errors = new[] { "PdaId должен быть уникальным." } });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToArray();
                Console.WriteLine("Ошибки в модели: " + string.Join(", ", errors));
                return Json(new { success = false, errors });
            }

            // Сохраняем в базу данных
            _context.Stalkers.Add(stalker);
            await _context.SaveChangesAsync();

            //------------------------------------------------------LOGS
            // Сохраняем нового сталкера в базу данных
            // Логируем изменение
            var changeLog = new ChangeLog
            {
                Action = "Добавление",
                EntityType = "Сталкер",
                EntityName = stalker.PdaId,
                ChangedBy = _httpContextAccessor.HttpContext.Session.GetString("UserRole"),
                ChangeTime = DateTime.Now
            };
            await _changeLogService.LogChangeAsync(changeLog);

            //------------------------------------------------------LOGS


            return Json(new { success = true });
        }

        // POST: Stalkers/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var stalker = await _context.Stalkers
                .Include(s => s.Quests)
                .FirstOrDefaultAsync(s => s.StalkerId == id);

            if (stalker != null)
            {
                var clansWithLeader = await _context.Clans
                    .Where(c => c.LeaderStalkerId == id)
                    .ToListAsync();

                foreach (var clan in clansWithLeader)
                {
                    clan.LeaderStalkerId = null; // Устанавливаем LeaderStalkerId в NULL
                }

                _context.Quests.RemoveRange(stalker.Quests);
                _context.Stalkers.Remove(stalker);
                await _context.SaveChangesAsync();

                //------------------------------------------------------LOGS
                // Логируем удаление
                var changeLog = new ChangeLog
                {
                    Action = "Удаление",
                    EntityType = "Сталкер",
                    EntityName = stalker.PdaId, // Можно использовать PdaId для идентификации
                    ChangedBy = _httpContextAccessor.HttpContext.Session.GetString("UserRole"),
                    ChangeTime = DateTime.Now
                };
                await _changeLogService.LogChangeAsync(changeLog);
                //------------------------------------------------------LOGS

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Сталкер не найден" });
        }



        [HttpPost]
        public async Task<IActionResult> UploadAvatar(int stalkerId, IFormFile avatarFile)
        {
            if (avatarFile != null && avatarFile.Length > 0)
            {
                var fileName = Path.GetFileName(avatarFile.FileName);
                var rootPath = @"C:\Users\danil\source\repos\DataBaze\DataBaze\wwwroot\avatars"; // Путь для сохранения
                var filePath = Path.Combine(rootPath, fileName);

                // Убедитесь, что папка существует
                if (!Directory.Exists(rootPath))
                {
                    Directory.CreateDirectory(rootPath);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await avatarFile.CopyToAsync(stream);
                }

                // Проверяем, существует ли уже аватар для данного сталкера
                var existingAvatar = await _context.Avatars.FirstOrDefaultAsync(a => a.StalkerId == stalkerId);
                if (existingAvatar != null)
                {
                    // Если аватар существует, обновляем его
                    existingAvatar.ImagePath = $"/avatars/{fileName}"; // Обновляем путь к изображению
                    _context.Avatars.Update(existingAvatar);
                    Console.WriteLine($"Обновлен путь к аватару: {existingAvatar.ImagePath}"); // Отладочная информация
                }
                else
                {
                    // Создаем новый объект Avatar и сохраняем его в базе данных
                    var avatar = new Avatar
                    {
                        ImagePath = $"/avatars/{fileName}",
                        StalkerId = stalkerId
                    };
                    _context.Avatars.Add(avatar);
                    Console.WriteLine($"Добавлен новый аватар с путем: {avatar.ImagePath}"); // Отладочная информация
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true, imagePath = $"/avatars/{fileName}" });
            }

            return Json(new { success = false, message = "Файл не был загружен." });
        }



    }

    // DTO для передачи данных
    public class StalkerDto
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Nickname { get; set; }
        public string PdaId { get; set; }
        public int ClanId { get; set; }
        public string Location { get; set; }
        public int RankId { get; set; }
    }
}
