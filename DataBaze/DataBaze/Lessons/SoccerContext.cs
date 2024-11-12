//using ConsoleApp20.Models;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ConsoleApp20.DAL
//{
//	public class SoccerContext : DbContext
//	{
//		public DbSet<Player> Players { get; set; } = null!;
//		public DbSet<Team> Teams { get; set; } = null!;
//		public DbSet<Coach> Coaches { get; set; } = null!;

//		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//		{
//			Console.WriteLine("Настройка подключения к базе данных...");

//			// Настраиваем строку подключения и включаем повторные попытки
//			optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=SoccerDb;Trusted_Connection=True;");


//			Console.WriteLine("Подключение настроено");
//		}

//	}
//}
