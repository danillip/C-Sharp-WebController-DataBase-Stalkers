//using ConsoleApp20.DAL;

//internal class Program
//{
//	static void Main(string[] args)
//	{
//		Console.WriteLine("Программа началась");

//		SoccerDBStorage soccerDBStorage = new SoccerDBStorage(new SoccerContext());

//		soccerDBStorage.EnsureDatabaseCreated();

//		Console.WriteLine("Контекст базы данных создан");

//		// Проверка на подключение к базе данных и наличие игроков
//		try
//		{
//			var playerList = soccerDBStorage.GetAllPlayers(); // Вызываем метод, чтобы получить всех игроков
//			if (playerList != null && playerList.Any())
//			{
//				Console.WriteLine("Игроки найдены");
//			}
//			else
//			{
//				Console.WriteLine("Игроков нет или не удалось подключиться к базе данных");
//			}
//		}
//		catch (Exception ex)
//		{
//			Console.WriteLine($"Ошибка при получении игроков: {ex.Message}");
//		}

//		Console.WriteLine("Программа завершена");
//	}
//}


