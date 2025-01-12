using Battleships.Models;

namespace Battleships.Models.Games
{
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

        // Saves current game state
        public IGameState Save()
        {
            Console.WriteLine("Current state: " + status + "saved");
            return new GameState(this.gameId, this.player1, this.player2, this.currentTurn, this.status, this.board1, this.board2);
        }

        // Restores the state
        public void Restore(IGameState iGameState) 
        {
            if (iGameState is not GameState gameState )
            {
                throw new ArgumentException("Invalid type.");
            }

            this.gameId = gameState.gameId;
            this.player1 = gameState.player1;
            this.player2 = gameState.player2;
            this.currentTurn = gameState.currentTurn;
            this.status = gameState.status;
            this.board1 = gameState.board1;
            this.board2 = gameState.board2; 
            Console.WriteLine("State restored:" + this.status);
        }

        // Private memento class
        private class GameState : IGameState
        {
            public int gameId { get; }
            public Player player1 { get; }
            public Player player2 { get; }
            public Player currentTurn { get; }
            public string status { get; }
            public Board board1 { get; }
            public Board board2 { get; }

            public GameState(int gameId, Player player1, Player player2, Player currentTurn, string status, Board board1, Board board2)
            {
                this.gameId = gameId;
                this.player1 = player1;
                this.player2 = player2;
                this.currentTurn = currentTurn;
                this.status = status;
                this.board1 = board1; 
                this.board2 = board2; 
            }
        }    
    }
}