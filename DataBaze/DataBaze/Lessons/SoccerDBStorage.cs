//using ConsoleApp20.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ConsoleApp20.DAL
//{
//    public class SoccerDBStorage
//    {
//        private readonly SoccerContext _context;

//        public SoccerDBStorage(SoccerContext soccerContext)
//        {
//            _context = soccerContext;

//        }

//		public void EnsureDatabaseCreated()
//		{
//			_context.Database.EnsureCreated();
//		}

//		public void AddPlayer(Player player)
//        {
//            _context.Players.Add(player);
//            _context.SaveChanges();
//        }
//        public Player? GetPlayer(int id)
//        {
//            Player? player = _context.Players.FirstOrDefault(x => x.PlayerId == id);
//            return player;
//        }
//        public List<Player> GetPlayers(string Name)
//        {
//            var player = _context.Players.Where(x => x.PlayerName == Name).ToList();
//            return player;
//        }
//        public void RemovePlayer(Player player)
//        {
//            _context.Players.Remove(player);
//            _context.SaveChanges();
//        }
//        public void RemovePlayer(string playerName)
//        {

//            _context.Players.RemoveRange(GetPlayers(playerName));
//            _context.SaveChanges();
//        }
//		public List<Player> GetAllPlayers()
//		{
//			return _context.Players.ToList();
//		}


//	}
//}
