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
            // player is given a new Board a ships
            Board = new Board();
            Ships = new List<IShip>();
            IsTurn = false;
        }
        // implementation of placeShip method

        // implementation of makeMove method
    }
}