namespace Battleships.Models
{
    public class ListOfPlayers
    {
        private List<Player> players;

        public ListOfPlayers()
        {
            players = new List<Player>();
        }

        public void AddPlayer(Player player)
        {
            player.playerId = GetNextPlayerId();
            players.Add(player);
        }

        public List<Player> GetPlayers()
        {
            return players;
        }

        private int GetNextPlayerId()
        {
            // Generates the next ID based on the existing players in the list
            return players.Count == 0 ? 1 : players[players.Count - 1].playerId + 1;
        }
    }
}