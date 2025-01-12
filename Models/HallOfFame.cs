using System.Collections.Generic;
using System.Linq;

namespace Battleship.Models
{
    public class HallOfFame
    {
        private static HallOfFame _instance;
        private static readonly object _lock = new object();

        public List<Statistics> PlayerStatistics { get; private set; }

        private HallOfFame()
        {
            PlayerStatistics = new List<Statistics>();
        }

        public static HallOfFame GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new HallOfFame();
                    }
                }
            }
            return _instance;
        }

        public List<Statistics> TopThree()
        {
            return PlayerStatistics
                .OrderByDescending(stats => stats.GamesWon)
                .Take(3)
                .ToList();
        }

        public List<Statistics> GetPlayerStatistics()
        {
            return PlayerStatistics;
        }

        public void AddOrUpdatePlayerStatistics(int playerId, bool isWin)
        {
            var playerStats = PlayerStatistics.FirstOrDefault(stats => stats.PlayerID == playerId);
            if (playerStats == null)
            {
                playerStats = new Statistics(playerId);
                PlayerStatistics.Add(playerStats);
            }
            playerStats.RecordResult(isWin);
        }
    }
}
