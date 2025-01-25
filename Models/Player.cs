namespace Battleships.Models
{
    public class Player
    {
        public int PlayerId { get; set; }
        public String Name { get; set; }
        public Board Board { get; set; }
        // public List<Ship> ships { get; set; }
        public bool IsTurn { get; set; }

        public Player(string name)
        {
            Name = name;
            // player is given a new Board a ships
            Board = new Board();
            // this.ships = new List<Ship>();
            IsTurn = false;
        }
        // implementation of placeShip method

        // implementation of makeMove method
    }
}