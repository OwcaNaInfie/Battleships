using System.Collections.Generic;
using System.Linq;

namespace Battleship.Models
{
    public class HallOfFame
    {
        private static HallOfFame Instance;
        private static readonly object Lock = new object();

        public List<Statistics> PlayerStatistics { get; private set; }

        private HallOfFame()
        {
            PlayerStatistics = new List<Statistics>();
        }

        public static HallOfFame GetInstance()
        {
            if (Instance == null)
            {
                lock (Lock)
                {
                    if (Instance == null)
                    {
                        Instance = new HallOfFame();
                    }
                }
            }
            return Instance;
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
