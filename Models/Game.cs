public class Game
{
    public int gameId { get; set; }
    public Player player1 { get; set; }
    public Player player2 { get; set; }
    public Player currentTurn { get; set; }
    public string status { get; set; }
    public Board board1 { get; set; }
    public Board board2 { get; set; }

    public Game(int gameId, string player1Name, string player2Name)
    {
        this.gameId = gameId;
        this.player1 = new Player(player1Name);
        this.player2 = new Player(player2Name);
        this.board1 = player1.board;
        this.board2 = player2.board;
        this.status = "Started";
    }

    // Starts the game and randomly assigns the first turn
    public void StartGame()
    {
        Random random = new Random();
        currentTurn = random.Next(0, 2) == 0 ? player1 : player2;
        status = $"{currentTurn.name}'s turn";
    }

    // Switches the turn to the other player
    public void SwitchTurn()
    {
        currentTurn = (currentTurn == player1) ? player2 : player1;
        status = $"{currentTurn.name}'s turn";
    }

    // Checks if either player has won
    public Player CheckWinner()
    {
        // implement how to check if a player is a winner 

        // if ()
        // {
        //     return player2;
        // }
        // else if ()
        // {
        //     return player1;
        // }
        return null; // No winner yet
    }

    public IGameState Save() {
        
    }
}
