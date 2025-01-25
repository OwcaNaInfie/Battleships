using Battleships.Models;

namespace Battleships.Models.Games
{
    public class Game
    {
        public int GameId { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Player CurrentTurn { get; set; }
        public string status { get; set; }
        public Board board1 { get; set; }
        public Board board2 { get; set; }

        public Game(int gameId, string Player1Name, string Player2Name)
        {
            this.GameId = gameId;
            this.Player1 = new Player(Player1Name);
            this.Player2 = new Player(Player2Name);
            this.board1 = Player1.board;
            this.board2 = Player2.board;
            this.status = "Started";
        }

        // Starts the game and randomly assigns the first turn
        public void StartGame()
        {
            Random random = new Random();
            CurrentTurn = random.Next(0, 2) == 0 ? Player1 : Player2;
            status = $"{CurrentTurn.name}'s turn";
        }

        // Switches the turn to the other player
        public void SwitchTurn()
        {
            CurrentTurn = (CurrentTurn == Player1) ? Player2 : Player1;
            status = $"{CurrentTurn.name}'s turn";
        }

        // Checks if either player has won
        public Player CheckWinner()
        {
            // implement how to check if a player is a winner 

            // if ()
            // {
            //     return Player2;
            // }
            // else if ()
            // {
            //     return Player1;
            // }
            return null; // No winner yet
        }

        // Saves current game state
        public IGameState Save()
        {
            Console.WriteLine("Current state: " + status + "saved");
            return new GameState(this.GameId, this.Player1, this.Player2, this.CurrentTurn, this.status, this.board1, this.board2);
        }

        // Restores the state
        public void Restore(IGameState iGameState) 
        {
            if (iGameState is not GameState gameState )
            {
                throw new ArgumentException("Invalid type.");
            }

            this.GameId = gameState.GameId;
            this.Player1 = gameState.Player1;
            this.Player2 = gameState.Player2;
            this.CurrentTurn = gameState.CurrentTurn;
            this.status = gameState.status;
            this.board1 = gameState.board1;
            this.board2 = gameState.board2; 
            Console.WriteLine("State restored:" + this.status);
        }

        // Private memento class
        private class GameState : IGameState
        {
            public int GameId { get; }
            public Player Player1 { get; }
            public Player Player2 { get; }
            public Player CurrentTurn { get; }
            public string status { get; }
            public Board board1 { get; }
            public Board board2 { get; }

            public GameState(int GameId, Player Player1, Player Player2, Player CurrentTurn, string status, Board board1, Board board2)
            {
                this.GameId = GameId;
                this.Player1 = Player1;
                this.Player2 = Player2;
                this.CurrentTurn = CurrentTurn;
                this.status = status;
                this.board1 = board1; 
                this.board2 = board2; 
            }
        }    
    }
}