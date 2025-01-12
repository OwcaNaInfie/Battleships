public class GameState : IGameState
{
    public int gameId { get; }
    public Player player1 { get; }
    public Player player2 { get; }
    public Player currentTurn { get; }
    public string status { get; }
    public Board board1State { get; }
    public Board board2State { get; }

    public GameState(Game game)
    {
        this.gameId = game.gameId;
        this.player1Name = game.player1.name;
        this.player2Name = game.player2.name;
        this.currentTurnName = game.currentTurn.name;
        this.status = game.status;
        // boardState to improve, how do we get the state of the board?
        this.board1State = game.board1; 
        this.board2State = game.board2; 
    }
}
