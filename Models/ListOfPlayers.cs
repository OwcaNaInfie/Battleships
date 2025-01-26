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

    }
}