namespace Battleships.Models
{
    public class Statistics
    {
        public int PlayerID { get; set; }
        public int GamesPlayed { get; private set; }
        public int GamesWon { get; private set; }
        public int GamesLost { get; private set; }

        public Statistics(int playerId)
        {
            PlayerID = playerId;
            GamesPlayed = 0;
            GamesWon = 0;
            GamesLost = 0;
        }

        public void RecordResult(bool isWin)
        {
            GamesPlayed++;
            if (isWin)
            {
                GamesWon++;
            }
            else
            {
                GamesLost++;
            }
        }
    }
}