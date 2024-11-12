using DataBaze.Stalker.Models;
using System.Linq;
using DataBaze.Stalker.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

public class HomeController : Controller
{
    private readonly StalkerContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    // Конструктор с добавлением IHttpContextAccessor для доступа к данным о текущем пользователе
    public HomeController(StalkerContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

	public IActionResult Index(int newsPage = 1, int changeLogPage = 1)
	{
		const int pageSize = 5; // Количество записей на странице

		// Пагинация для новостей
		var newsCount = _context.News.Count();
		var totalNewsPages = (int)Math.Ceiling(newsCount / (double)pageSize);
		var news = _context.News
						   .OrderByDescending(n => n.CreatedAt)
						   .Skip((newsPage - 1) * pageSize)
						   .Take(pageSize)
						   .ToList();

		// Пагинация для журнала изменений
		var changeLogsCount = _context.ChangeLogs.Count();
		var totalChangeLogsPages = (int)Math.Ceiling(changeLogsCount / (double)pageSize);
		var changeLogs = _context.ChangeLogs
								 .OrderByDescending(c => c.ChangeTime)
								 .Skip((changeLogPage - 1) * pageSize)
								 .Take(pageSize)
								 .ToList();

		// Передаем данные в представление
		ViewBag.News = news;
		ViewBag.NewsCurrentPage = newsPage;
		ViewBag.NewsTotalPages = totalNewsPages;

		ViewBag.ChangeLogs = changeLogs;
		ViewBag.ChangeLogsCurrentPage = changeLogPage;
		ViewBag.ChangeLogsTotalPages = totalChangeLogsPages;

		return View();
	}

	// Переименовали метод в GetNewsDetails
	[HttpGet]
    public IActionResult GetNewsDetails(int id)
    {
        var news = _context.News
            .Where(n => n.Id == id)
            .FirstOrDefault();

        if (news != null)
        {
            Console.WriteLine("Контент новости: " + news.Content); // Для отладки
            return Json(new { success = true, Content = news.Content });
        }

        return Json(new { success = false });
    }
    // Метод для добавления новости с логированием
    [HttpPost]
    public async Task<IActionResult> AddNews(News news)
    {
        if (ModelState.IsValid)
        {
            news.CreatedAt = DateTime.Now;  // Устанавливаем текущую дату

            _context.News.Add(news);  // Добавляем новость в базу данных
            await _context.SaveChangesAsync();  // Сохраняем изменения

            // Логируем добавление новости
            var changeLog = new ChangeLog
            {
                Action = "Добавление",
                EntityType = "Новость",
                EntityName = news.Title,
                ChangedBy = _httpContextAccessor.HttpContext.Session.GetString("UserRole"),
                ChangeTime = DateTime.Now
            };
            _context.ChangeLogs.Add(changeLog);  // Добавляем запись в журнал изменений
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
        return Json(new { success = false });
    }

    // Метод для удаления новости с логированием
    [HttpPost]
    public IActionResult DeleteNews(int id)
    {
        var newsItem = _context.News.FirstOrDefault(n => n.Id == id);
        if (newsItem != null)
        {
            // Логируем удаление новости
            var changeLog = new ChangeLog
            {
                Action = "Удаление",
                EntityType = "Новость",
                EntityName = newsItem.Title,
                ChangedBy = _httpContextAccessor.HttpContext.Session.GetString("UserRole"),
                ChangeTime = DateTime.Now
            };
            _context.ChangeLogs.Add(changeLog);  // Добавляем запись в журнал изменений
            _context.News.Remove(newsItem);  // Удаляем новость из базы данных
            _context.SaveChanges();

            return Json(new { success = true });
        }
        return Json(new { success = false });
    }
}

