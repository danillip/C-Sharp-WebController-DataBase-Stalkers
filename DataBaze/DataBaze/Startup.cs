using DataBaze.Stalker.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using DataBaze.Services;
using Microsoft.Extensions.FileProviders;

namespace DataBaze
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Настройка подключения к базе данных
            services.AddDbContext<StalkerContext>(options =>
                options.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=StalkerDb;Trusted_Connection=True;MultipleActiveResultSets=True;"));

            // Добавление поддержки сессий
            services.AddSession();

            // Logi
            services.AddScoped<ChangeLogService>();

            // Поддержка CORS для разрешения запросов с любых источников
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            // Поддержка компиляции представлений во время выполнения
            services.AddControllersWithViews()
                    .AddRazorRuntimeCompilation();

            // Поддержка доступа к контексту HTTP (например, для сессий)
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public void Configure(IApplicationBuilder app, ILogger<Startup> logger)
        {
            // Обслуживание всех статических файлов из папки wwwroot
            app.UseStaticFiles();

            // Настройка для обслуживания файлов из папки wwwroot/avatars
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars")),
                RequestPath = "/avatars"
            });

            app.UseRouting();

            // Включение CORS
            app.UseCors("AllowAllOrigins");

            // Добавление middleware для сессий
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "auth",
                    pattern: "Auth/{action=Login}",
                    defaults: new { controller = "Auth" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // Логирование для отладки
            app.Use(async (context, next) =>
            {
                logger.LogInformation("Обработка запроса: {0}", context.Request.Path);
                await next.Invoke();
                logger.LogInformation("Запрос обработан: {0}", context.Response.StatusCode);
            });
        }
    }
}
