using Battleships.Models.Ships;
namespace Battleships.Models
{
    public class Player
    {
        public int PlayerId { get; set; }
        public String Name { get; set; }
        public Board Board { get; set; }
        public List<IShip> Ships { get; set; }
        public bool IsTurn { get; set; }
   

        public Player(string name)
        {
            Name = name;
            Board = new Board();
            Ships = new List<IShip>();
            IsTurn = false;
            PlayerId = PlayerIdGenerator.GetNewId();
        
        }
    }

    public static class PlayerIdGenerator
    {
        private static int Counter { get; set; }
        public static int GetNewId() { Counter++; return Counter; }
    }
}