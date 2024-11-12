using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using DataBaze;
using DataBaze.Stalker.Data;

namespace StalkerDb
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Настраиваем и запускаем веб-хост
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseContentRoot("C:\\Users\\danil\\source\\repos\\DataBaze\\DataBaze"); // Изменить на свою корневую папку
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://localhost:5000");  // Адрес для запуска веб-интерфейса

                })
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<StalkerContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=StalkerDb;Trusted_Connection=True;MultipleActiveResultSets=True;");

            using (var context = new StalkerContext(optionsBuilder.Options))
            {
                try
                {
                    // Удаляем базу данных, если она существует
                    // context.Database.EnsureDeleted();
                    // Console.WriteLine("Старая база данных удалена.");

                    // Создаем базу данных и таблицы
                    context.Database.EnsureCreated();
                    Console.WriteLine("База данных и таблицы созданы.");

                    // Применяем миграции
                    context.Database.Migrate();
                    Console.WriteLine("Миграции применены.");

                    // Теперь заполняем базу данными
                    await DataSeeder.SeedData(context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                    Console.WriteLine(ex.InnerException?.Message);
                }
            }

            // Запуск веб-интерфейса
            await host.RunAsync();
        }
    }
}