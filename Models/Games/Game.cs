using Battleships.Controllers;
using Battleships.Models;
using Battleships.Models.Ships;

namespace Battleships.Models.Games
{
    public class Game
    {
        public Player Winner { get; private set; }
        public int GameId { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Player CurrentTurn { get; set; }
        public string Status { get; set; }
        public Board Board1 { get; set; }
        public Board Board2 { get; set; }
        public CommandManager CommandInvoker { get; } = new CommandManager();

        public Game(int gameId, string player1Name, string player2Name, char player1ShipSymbol, char player2ShipSymbol)
        {
            GameId = gameId;
            Player1 = new Player(player1Name, player1ShipSymbol);
            Player2 = new Player(player2Name, player2ShipSymbol);
            Board1 = Player1.Board;
            Board2 = Player2.Board;
            CurrentTurn = Player1;
            Status = "Started";
        }

        // Metoda rozpoczynająca grę
        public void StartGame()
        {
            Console.WriteLine("Game Started!");
        }

        // Metoda zmieniająca kolej gracza
        public void SwitchTurn()
        {
            CurrentTurn = (CurrentTurn == Player1) ? Player2 : Player1;
            Status = $"{CurrentTurn.Name}'s turn";
        }

        public Board GetOpponentBoard()
        {
            return (CurrentTurn == Player1) ? Player2.Board : Player1.Board;
        }

        public Board GetCurrentBoard()
        {
            return (CurrentTurn == Player1) ? Player1.Board : Player2.Board;
        }

        public Player? CheckWinner()
        {
            if (Player1.Board.AreAllShipsSunk())
            {
                return Player2; // Gracz 2 wygrał
            }
            if (Player2.Board.AreAllShipsSunk())
            {
                return Player1; // Gracz 1 wygrał
            }
            return null; // Nie ma wygranego
        }


        // Zapisanie stanu aktualnej gry
        public IGameState Save()
        {
            Console.WriteLine("Current state: " + Status + "saved");
            return new GameState(this.GameId, this.Player1, this.Player2, this.CurrentTurn, this.Status, this.Board1, this.Board2);
        }

        // Zwrócenie stanu aktulnej gry
        public void Restore(IGameState iGameState)
        {
            if (iGameState is not GameState gameState)
            {
                throw new ArgumentException("Invalid type.");
            }

            this.GameId = gameState.GameId;
            this.Player1 = gameState.Player1;
            this.Player2 = gameState.Player2;
            this.CurrentTurn = gameState.CurrentTurn;
            this.Status = gameState.Status;
            this.Board1 = gameState.Board1;
            this.Board2 = gameState.Board2;
            Console.WriteLine("State restored:" + this.Status);
        }

        // Memento
        private class GameState : IGameState
        {
            public int GameId { get; }
            public Player Player1 { get; }
            public Player Player2 { get; }
            public Player CurrentTurn { get; }
            public string Status { get; }
            public Board Board1 { get; }
            public Board Board2 { get; }

            public GameState(int gameId, Player player1, Player player2, Player currentTurn, string status, Board board1, Board board2)
            {
                GameId = gameId;
                Player1 = player1;
                Player2 = player2;
                CurrentTurn = currentTurn;
                Status = status;
                Board1 = board1;
                Board2 = board2;
            }
        }
    }
}
