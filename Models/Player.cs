public class Player
{
    public int playerId { get; set; }
    public String name { get; set; }
    public Board board { get; set; }
    public List<Ship> ships { get; set; }
    public bool isTurn { get; set; }

    public Player(string name)
    {
        this.name = name;
        // player is given a new Board a ships
        this.board = new Board();
        this.ships = new List<Ship>();
        this.isTurn = false;
    }
    // implementation of placeShip method

    // implementation of makeMove method
}