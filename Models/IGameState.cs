public class IGameState
{
    public int gameId { get; }
    public Player player1 { get; }
    public Player player2 { get; }
    public Player currentTurn { get; }
    public string status { get; }
    public Board board1State { get; }
    public Board board2State { get; }

}
