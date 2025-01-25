using Battleships.Models;
using Battleships.Models.Ships;

namespace Battleships.Models.Games
{
    public class Game
    {
        public int GameId { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Player CurrentTurn { get; set; }
        public string Status { get; set; }
        public Board Board1 { get; set; }
        public Board Board2 { get; set; }

        public Game(int gameId, string player1Name, string player2Name)
        {
            GameId = gameId;
            Player1 = new Player(player1Name);
            Player2 = new Player(player2Name);
            Board1 = Player1.Board;
            Board2 = Player2.Board;
            Status = "Started";
        }

        // Starts the game and randomly assigns the first turn
        public void StartGame()
        {
            Console.WriteLine("Game Started!");
            Player1TurnToPlaceShips();
            Player2TurnToPlaceShips();
        }

        private void Player1TurnToPlaceShips()
        {
            Console.WriteLine($"{Player1.Name}, it's your turn to place ships!");

            // Ship placement loop
            PlaceShip(Player1);
        }

        private void Player2TurnToPlaceShips()
        {
            Console.WriteLine($"{Player2.Name}, it's your turn to place ships!");

            // Ship placement loop
            PlaceShip(Player2);
        }

        private void PlaceShip(Player player)
        {
            // Define available ship factories
            List<ShipFactory> factories = new List<ShipFactory>
            {
                OneMastFactory.Instance,
                TwoMastFactory.Instance,
                ThreeMastFactory.Instance,
                FourMastFactory.Instance
            };

            // Loop through available ships (allow player to place ships)
            foreach (var factory in factories)
            {
                while (true)
                {
                    Console.WriteLine($"Place your {factory.CreateShip(1).Size}-mast ship.");
                    Console.WriteLine("Enter the starting row (0 to 9):");
                    int row = int.Parse(Console.ReadLine());

                    Console.WriteLine("Enter the starting column (0 to 9):");
                    int col = int.Parse(Console.ReadLine());

                    Console.WriteLine("Enter the orientation (H for Horizontal, V for Vertical):");
                    string orientationInput = Console.ReadLine().ToUpper();
                    Orientation orientation = orientationInput == "H" ? Orientation.Horizontal : Orientation.Vertical;

                    IShip newShip = factory.CreateShip(player == Player1 ? 1 : 2);
                    bool shipPlaced = player.Board.PlaceShip(newShip, row, col, orientation);

                    if (shipPlaced)
                    {
                        Console.WriteLine($"{player.Name}, your ship has been placed successfully.");
                        player.Ships.Add(newShip);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid placement, please try again.");
                    }
                }
            }
        }

        // Switches the turn to the other player
        public void SwitchTurn()
        {
            CurrentTurn = (CurrentTurn == Player1) ? Player2 : Player1;
            Status = $"{CurrentTurn.Name}'s turn";
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
            Console.WriteLine("Current state: " + Status + "saved");
            return new GameState(this.GameId, this.Player1, this.Player2, this.CurrentTurn, this.Status, this.Board1, this.Board2);
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
            this.Status = gameState.Status;
            this.Board1 = gameState.Board1;
            this.Board2 = gameState.Board2; 
            Console.WriteLine("State restored:" + this.Status);
        }

        // Private memento class
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