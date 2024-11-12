using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataBaze.Stalker.Models;
using DataBaze.Stalker.Data;

namespace StalkerDb
{
    public static class DataSeeder
    {
        public static async Task SeedData(StalkerContext context)
        {
            // Проверяем, существуют ли уже данные, добавлять все данные
            if (await context.Stalkers.AnyAsync() || await context.Ranks.AnyAsync() || await context.Clans.AnyAsync())
            {
                return; // Если данные уже есть, ничего не делаем
            }

            // Создаем данные для таблицы Ranks без указания RankId
            var ranks = new List<Rank>
            {
                new Rank { RankName = "Новичок", MutantsKilledRequired = "10" },
                new Rank { RankName = "Опытный", MutantsKilledRequired = "50" },
                new Rank { RankName = "Мастер", MutantsKilledRequired = "100" }
            };

            await context.Ranks.AddRangeAsync(ranks);
            await context.SaveChangesAsync(); // Сохраняем изменения

            // Получаем все доступные ранги
            //var rankList = await context.Ranks.ToListAsync();

            // Создаем данные для таблицы Clans
            var clans = new List<Clan>
            {
                new Clan { Name = "Святые", Location = "Заброшенный храм" },
                new Clan { Name = "Гроза", Location = "Промзона" },
                new Clan { Name = "Скауты", Location = "Лес" }
            };

            await context.Clans.AddRangeAsync(clans);
            await context.SaveChangesAsync(); // Сохраняе м изменения

            // Создаем данные для таблицы KlanRelationships
            var klanRelationships = new List<KlanRelationship>
            {
                new KlanRelationship { ClanId1 = 1, ClanId2 = 2, RelationshipType = "вражда", Description = "Клан 'Долг' противостоит 'Свободе'" },
                new KlanRelationship { ClanId1 = 1, ClanId2 = 3, RelationshipType = "союз", Description = "Клан 'Долг' в научных интересах с 'Ренегаты'" },
                new KlanRelationship { ClanId1 = 2, ClanId2 = 3, RelationshipType = "союз", Description = "Клан 'Свобода' в продовольственном союзе с 'Ренегатами'" }
            };

            // Добавляем данные в базу данных
            await context.KlanRelationships.AddRangeAsync(klanRelationships);
            await context.SaveChangesAsync(); // Сохраняем изменения

            // Создаем данные для таблицы Stalkers, связываем RankId
            var stalkers = new List<Stalker>
            {
                new Stalker { LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Nickname = "Лидер", PdaId = "PDA001", ClanId = 1, Location = "Бар", RankId = 1 },
                new Stalker { LastName = "Петров", FirstName = "Петр", MiddleName = "Петрович", Nickname = "Странник", PdaId = "PDA002", ClanId = 1, Location = "Деревня", RankId = 1 },
                new Stalker { LastName = "Сидоров", FirstName = "Сидор", MiddleName = "Сидорович", Nickname = "Снайпер", PdaId = "PDA003", ClanId = 2, Location = "Кладбище", RankId = 3 }
            };

            await context.Stalkers.AddRangeAsync(stalkers);
            await context.SaveChangesAsync(); // Сохраняем изменения

            // Назначаем лидеров кланов (первого сталкера в каждом клане) При условии что включен многопоток в мейне
            foreach (var clan in context.Clans)
            {
                // Получаем первого сталкера, который вошел в клан
                var leader = context.Stalkers.FirstOrDefault(s => s.ClanId == clan.ClanId);

                if (leader != null)
                {
                    clan.LeaderStalkerId = leader.StalkerId; // Назначаем лидера
                }
            }

            await context.SaveChangesAsync(); // Сохраняем изменения


            // Создаем данные для таблицы Weapon
            var weapons = new List<Weapon>
            {
                new Weapon
                {
                    Name = "AK-74",
                    Type = "Автомат",
                    FireRate = 600,
                    Range = 300,
                    MagazineSize = 30,
                    Description = "Надежный автомат, популярный среди сталкеров."
                },
                new Weapon
                {
                    Name = "MP5",
                    Type = "Пистолет-пулемет",
                    FireRate = 800,
                    Range = 200,
                    MagazineSize = 30,
                    Description = "Компактный и эффективный пистолет-пулемет для ближнего боя."
                },
                new Weapon
                {
                    Name = "M4A1",
                    Type = "Автомат",
                    FireRate = 750,
                    Range = 400,
                    MagazineSize = 30,
                    Description = "Модернизированный автомат, высокоточный и универсальный."
                },
                new Weapon
                {
                    Name = "SVD",
                    Type = "Снайперская винтовка",
                    FireRate = 30,
                    Range = 800,
                    MagazineSize = 10,
                    Description = "Снайперская винтовка с высокой точностью и дальнобойностью."
                },
                new Weapon
                {
                    Name = "Пистолет Макарова",
                    Type = "Пистолет",
                    FireRate = 300,
                    Range = 50,
                    MagazineSize = 8,
                    Description = "Классический советский пистолет, подходящий для ближнего боя."
                }
            };

            // Добавляем данные в контекст
            await context.Weapons.AddRangeAsync(weapons);
            await context.SaveChangesAsync(); // Сохраняем изменения


            var artifacts = new List<Artifact>
            {
                new Artifact {Name = "Золотая Рыба", Cost = 100000, Description = "Редкий артефакт, увеличивающий выносливость." },
                new Artifact {Name = "Грави", Cost = 75000, Description = "Уменьшает вес предметов в рюкзаке." },
                new Artifact {Name = "Медуза", Cost = 50000, Description = "Снижает радиацию вокруг владельца." },
                new Artifact {Name = "Колобок", Cost = 120000, Description = "Увеличивает регенерацию здоровья." },
                new Artifact {Name = "Выверт", Cost = 95000, Description = "Повышает защиту от физических повреждений." }
            };
            // Добавляем записи в контекст базы данных
            context.Artifacts.AddRange(artifacts);
            await context.SaveChangesAsync(); // Сохраняем изменения

            // Создаем данные для таблицы Quest
            var quests = new List<Quest>
            {
                new Quest
                {
                    StalkerId = 1, // ID сталкера, который принимает квест
                    ArtifactId = 1, // ID артефакта, связанного с квестом
                    RewardWeaponId = 1, // ID оружия, получаемого в награду
                    Description = "Принести артефакт 'Светлячок' с болот."
                },
                new Quest
                {
                    StalkerId = 2, // ID сталкера
                    ArtifactId = 2, // ID артефакта
                    RewardWeaponId = 2, // ID оружия
                    Description = "Уничтожить группу мутантов на окраине Зоны."
                },
                new Quest
                {
                    StalkerId = 3, // ID сталкера
                    ArtifactId = 1, // ID артефакта
                    RewardWeaponId = 3, // ID оружия
                    Description = "Собрать 3 артефакта 'Пуля'."
                },
                new Quest
                {
                    StalkerId = 1, // ID сталкера
                    ArtifactId = 3, // ID артефакта
                    RewardWeaponId = 4, // ID оружия
                    Description = "Исследовать аномалию на юге Зоны."
                },
                new Quest
                {
                    StalkerId = 2, // ID сталкера
                    ArtifactId = 2, // ID артефакта
                    RewardWeaponId = 5, // ID оружия
                    Description = "Помочь другим сталкерам в их поисках."
                }
            };

            // Добавляем данные в контекст
            await context.Quests.AddRangeAsync(quests);
            await context.SaveChangesAsync(); // Сохраняем изменения



            // Вставка данных в таблицу Ammunition
            var ammunitions = new List<Ammunition>
            {
                new Ammunition
                {
                    Description = "Патроны 7.62x39",
                    Caliber = "7.62x39",
                    Type = 1,  // Например, 1 может обозначать боевые патроны
                    WeaponId = 1  // Замените на существующий ID оружия
                },
                new Ammunition
                {
                    Description = "Патроны 9x19",
                    Caliber = "9x19",
                    Type = 2,  // Например, 2 может обозначать тренировочные патроны
                    WeaponId = 5  // Замените на существующий ID другого оружия
                },
                new Ammunition
                {
                    Description = "Патроны 5.56x45 NATO",
                    Caliber = "5.56x45",
                    Type = 1,
                    WeaponId = 3  // Замените на существующий ID третьего оружия
                }
            };

            // Добавляем записи в контекст базы данных
            context.Ammunitions.AddRange(ammunitions);
            await context.SaveChangesAsync(); // Сохраняем изменения

            var anomalies = new List<Anomaly>
            {
                new Anomaly {Name = "Жарка", Description = "Аномалия, испускающая экстремальное тепло." },
                new Anomaly {Name = "Воронка", Description = "Сильный гравитационный эффект, втягивающий всё внутрь." },
                new Anomaly {Name = "Карусель", Description = "Создаёт вращающийся воздушный вихрь, опасный для всего живого." },
                new Anomaly {Name = "Трамплин", Description = "Периодические выбросы энергии, подбрасывающие предметы и сталкеров." },
                new Anomaly {Name = "Электра", Description = "Произвольные электрические разряды, бьющие в случайные направления." }
            };
            // Добавляем записи в контекст базы данных
            context.Anomalies.AddRange(anomalies);
            await context.SaveChangesAsync(); // Сохраняем изменения

            var artifactAnomalies = new List<ArtifactAnomaly>
            {
                new ArtifactAnomaly {ArtifactId = 1, AnomalyId = 2, AppearanceDate = new DateTime(2024, 10, 15) },
                new ArtifactAnomaly {ArtifactId = 2, AnomalyId = 1, AppearanceDate = new DateTime(2024, 10, 16) },
                new ArtifactAnomaly {ArtifactId = 3, AnomalyId = 5, AppearanceDate = new DateTime(2024, 10, 17) },
                new ArtifactAnomaly {ArtifactId = 4, AnomalyId = 3, AppearanceDate = new DateTime(2024, 10, 18) },
                new ArtifactAnomaly {ArtifactId = 1, AnomalyId = 4, AppearanceDate = new DateTime(2024, 10, 19) },
                new ArtifactAnomaly {ArtifactId = 5, AnomalyId = 2, AppearanceDate = new DateTime(2024, 10, 20) },
                new ArtifactAnomaly {ArtifactId = 3, AnomalyId = 4, AppearanceDate = new DateTime(2024, 10, 21) }
            };
            // Добавляем записи в контекст базы данных
            context.ArtifactAnomalies.AddRange(artifactAnomalies);
            await context.SaveChangesAsync(); // Сохраняем изменения


            var users = new List<User>
            {
                new User { Username = "admin", Password = "password", Role = "Admin" },
                new User { Username = "danillip", Password = "password", Role = "User" }
            };

            // Добавляем пользователей в контекст базы данных
            context.Users.AddRange(users);
            await context.SaveChangesAsync(); // Сохраняем изменения

            var news = new List<News>
{
                new News
                {
                    Title = "Новость 1",
                    Summary = "Это краткое описание новости 1",
                    Content = "Полный текст новости 1. Здесь может быть много информации о новости.",
                    CreatedAt = DateTime.Now.AddDays(-1)
                },
                new News
                {
                    Title = "Новость 2",
                    Summary = "Это краткое описание новости 2",
                    Content = "Полный текст новости 2. Более подробная информация о событиях.",
                    CreatedAt = DateTime.Now.AddDays(-2)
                },
                new News
                {
                    Title = "Новость 3",
                    Summary = "Это краткое описание новости 3",
                    Content = "Полный текст новости 3. В этом разделе подробности о важном событии.",
                    CreatedAt = DateTime.Now.AddDays(-3)
                }
            };

            // Добавляем новости в контекст базы данных
            context.News.AddRange(news);
            await context.SaveChangesAsync(); // Сохраняем изменения

        }

    }
}
