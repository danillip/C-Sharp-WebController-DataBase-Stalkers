using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaze.Stalker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace DataBaze.Stalker.Data
{
    public class StalkerContext : DbContext
    {
        public StalkerContext(DbContextOptions<StalkerContext> options) : base(options) { }

        public DbSet<ChangeLog> ChangeLogs { get; set; } //ЛОГИ
        public DbSet<Avatar> Avatars { get; set; } // АВАТАРКИ
        public DbSet<User> Users { get; set; } // Юзеры
        public DbSet<News> News { get; set; } // Новости

        public DbSet<DataBaze.Stalker.Models.Stalker> Stalkers { get; set; } // Свойство для таблицы Сталкера
        public DbSet<Rank> Ranks { get; set; } // Свойство для таблицы Сталкера
        public DbSet<Clan> Clans { get; set; } // Свойство для таблицы Кланов
        public DbSet<KlanRelationship> KlanRelationships { get; set; } // Свойство для таблицы Взаимоотношений
        public DbSet<Quest> Quests { get; set; } // Свойство для таблицы Квестов
        public DbSet<Weapon> Weapons { get; set; } // Свойство для таблицы Пушек
        public DbSet<Ammunition> Ammunitions { get; set; } // Свойство для таблицы Пушек
        public DbSet<Artifact> Artifacts { get; set; } // Свойство для таблицы Пушек
        public DbSet<Anomaly> Anomalies { get; set; } // Свойство для таблицы Аномалий
        public DbSet<ArtifactAnomaly> ArtifactAnomalies { get; set; } // Свойство для таблицы Артов в аномалиях 


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataBaze.Stalker.Models.Stalker>()
                .HasKey(s => s.StalkerId); // Задаем первичный ключ
            modelBuilder.Entity<DataBaze.Stalker.Models.Stalker>()
                .HasOne(s => s.Rank) // Один Сталкер имеет один Ранг
                .WithMany() // У Ранга может быть много Сталкеров
                .HasForeignKey(s => s.RankId); // Внешний ключ
            modelBuilder.Entity<DataBaze.Stalker.Models.Stalker>()
                .HasOne(s => s.Clan) // Один Сталкер имеет один Ранг
                .WithMany() // У Клана может быть много Сталкеров
                .HasForeignKey(s => s.ClanId); // Внешний ключ

            modelBuilder.Entity<Rank>()
                .HasKey(r => r.RankId); //Первичный ключ Рангам

            modelBuilder.Entity<Clan>() //Первичный ключ Кланам
                .HasKey(c => c.ClanId);
            modelBuilder.Entity<Clan>()
                .HasOne(c => c.Leader)  //Передаем сталера в лидеры клана см Klan.cs
                .WithOne() //У клана ТОЛЬКО один лидер
                .HasForeignKey<Clan>(c => c.LeaderStalkerId) // Внешний ключ в таблице Clan
                .OnDelete(DeleteBehavior.NoAction); // Отключаем каскадное удаление для этого ключа

            modelBuilder.Entity<KlanRelationship>()
                .HasKey(kr => kr.KlanRelationshipId); // Задаем первичный ключ
            // Настройка связи между KlanRelationship и Clan через ClanId1
            modelBuilder.Entity<KlanRelationship>()
                .HasOne(kr => kr.Clan1)               // Навигационное свойство
                .WithMany()                           // Один клан может участвовать в нескольких отношениях
                .HasForeignKey(kr => kr.ClanId1)      // Внешний ключ ClanId1
                .OnDelete(DeleteBehavior.Restrict);   // Настройка удаления: без каскадного удаления
            // Настройка связи между KlanRelationship и Clan через ClanId2
            modelBuilder.Entity<KlanRelationship>()
                .HasOne(kr => kr.Clan2)               // Навигационное свойство
                .WithMany()                           // Один клан может участвовать в нескольких отношениях
                .HasForeignKey(kr => kr.ClanId2)      // Внешний ключ ClanId2
                .OnDelete(DeleteBehavior.Restrict);   // Настройка удаления: без каскадного удаления


            // Настройка сущности Quest
            modelBuilder.Entity<Quest>()
                .HasKey(q => q.QuestId); // Устанавливаем первичный ключ для таблицы Quest
            // Настройка связи между Quest и Weapon
            modelBuilder.Entity<Quest>()
                .HasOne(q => q.RewardWeapon) // Один квест может иметь одно наградное оружие
                .WithMany() // У Weapon может быть много квестов (можно оставить пустым, если не требуется)
                .HasForeignKey(q => q.RewardWeaponId) // Указываем внешний ключ, который будет ссылаться на RewardWeaponId в таблице Quest
                .OnDelete(DeleteBehavior.Restrict); // Настройка поведения при удалении: Restrict — нельзя удалить оружие, если оно связано с квестом
            modelBuilder.Entity<Quest>() // Связь между Quest и Stalker
                .HasOne(q => q.Stalker) // Один квест связан с одним сталкером
                .WithMany(s => s.Quests) // У одного сталкера может быть много квестов
                .HasForeignKey(q => q.StalkerId) // Указываем внешний ключ
                .OnDelete(DeleteBehavior.Restrict); // Настройка поведения при удалении


            modelBuilder.Entity<Weapon>()
                .HasKey(w => w.WeaponId); //Первичный ключ Пухам
            modelBuilder.Entity<Quest>()
                .HasOne(q => q.RewardWeapon) // Один квест может иметь одно наградное оружие
                .WithMany() // У Weapon может быть много квестов
                .HasForeignKey(q => q.RewardWeaponId); // Указываем внешний ключ

            modelBuilder.Entity<Ammunition>()
                .HasKey(a => a.AmmunitionId); //Первичный ключ Патрикам
            modelBuilder.Entity<Ammunition>()
                .HasOne(a => a.Weapon)  // Связь с Weapon
                .WithMany(w => w.Ammunitions)  // У одного Weapon может быть несколько Ammunition
                .HasForeignKey(a => a.WeaponId)  // Внешний ключ для связи
                .OnDelete(DeleteBehavior.Restrict);  // Ограничение на удаление

            modelBuilder.Entity<Artifact>()
                .HasKey(ar => ar.ArtifactId); //Первичный ключ Артефактов

            modelBuilder.Entity<Anomaly>()
                .HasKey(an => an.AnomalyId); //Первичный ключ Аномалий

            modelBuilder.Entity<ArtifactAnomaly>()
                .HasKey(aa => aa.ArtifactAnomalyId);
            // Связь между Artifact и ArtifactAnomaly (Один артефакт может появляться в разных аномалиях)
            modelBuilder.Entity<ArtifactAnomaly>()
                .HasOne(aa => aa.Artifact)
                .WithMany() // Артефакт может быть связан с несколькими записями в ArtifactAnomaly
                .HasForeignKey(aa => aa.ArtifactId)
                .OnDelete(DeleteBehavior.Cascade); // Удаление артефакта приведет к удалению всех связанных записей в ArtifactAnomaly
                                                   // Связь между Anomaly и ArtifactAnomaly (Одна аномалия может содержать несколько артефактов)
            modelBuilder.Entity<ArtifactAnomaly>()
                .HasOne(aa => aa.Anomaly)
                .WithMany() // Аномалия может быть связана с несколькими записями в ArtifactAnomaly
                .HasForeignKey(aa => aa.AnomalyId)
                .OnDelete(DeleteBehavior.Cascade); // Удаление аномалии приведет к удалению всех связанных записей в ArtifactAnomaly
            
            modelBuilder.Entity<ChangeLog>()
                            .ToTable("ChangeLogs"); //ЛОГИ

            modelBuilder.Entity<Avatar>()
                .HasKey(a => a.Id); // Устанавливаем Id как первичный ключ

            modelBuilder.Entity<Avatar>()
                .Property(a => a.ImagePath)
                .IsRequired(); // Делаем поле ImagePath обязательным

            modelBuilder.Entity<Avatar>()
                .HasOne(a => a.Stalker) // Устанавливаем связь с моделью Stalker
                .WithOne(s => s.Avatar) // Указываем, что у сталкера может быть одна аватарка
                .HasForeignKey<Avatar>(a => a.StalkerId); // Устанавливаем внешний ключ
        }

    }
}
