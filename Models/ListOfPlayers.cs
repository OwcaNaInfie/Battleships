namespace Battleships.Models
{
    public class ListOfPlayers
    {
        private List<Player> Players;

        public ListOfPlayers()
        {
            this.Players = new List<Player>();
        }

        public void AddPlayer(Player player)
        {
            player.PlayerId = GetNextPlayerId();
            Players.Add(player);
        }

        public List<Player> GetPlayers()
        {
            return Players;
        }

        private int GetNextPlayerId()
        {
            // Generates the next ID based on the existing players in the list
            return Players.Count == 0 ? 1 : Players[Players.Count - 1].PlayerId + 1;
        }
    }
}