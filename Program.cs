using System;
using Battleships.Models;
using Battleships.Models.Ships;
using Battleships.Models.Games;

namespace Battleships
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Battleships!");
            Console.WriteLine("Please enter the name for Player 1:");
            string player1Name = Console.ReadLine();
            Console.WriteLine("Please enter the name for Player 2:");
            string player2Name = Console.ReadLine();

            // Create a new game and initialize players with PlayerId
            Game game = new Game(1, player1Name, player2Name);
            game.Player1 = new Player(player1Name);
            game.Player2 = new Player(player2Name);

            // Start the game
            game.StartGame();

            // Let each player place their ships
            PlaceShips(game.Player1);
            PlaceShips(game.Player2);

            // Start the game loop
            while (game.CheckWinner() == null)
            {
                Console.WriteLine(game.Status);
                // Implement gameplay loop here
                Attack();
                game.SwitchTurn();
            }

            // Once the game is over
            Player winner = game.CheckWinner();
            if (winner != null)
            {
                Console.WriteLine($"{winner.Name} wins the game!");
            }
            else
            {
                Console.WriteLine("Game over without a winner.");
            }
        }

        static void PlaceShips(Player player)
        {
            Console.WriteLine($"{player.Name}, it's time to place your ships!\n");

            player.Board.DisplayBoard();

            // Corrected number of ships
            PlaceShipForPlayer(player, 1, 4); // 4 ships of size 1-mast
            PlaceShipForPlayer(player, 2, 3); // 3 ships of size 2-mast
            PlaceShipForPlayer(player, 3, 2); // 2 ships of size 3-mast
            PlaceShipForPlayer(player, 4, 1); // 1 ship of size 4-mast

            Console.WriteLine($"{player.Name} has placed all ships.");
        }

        static void PlaceShipForPlayer(Player player, int shipSize, int count)
        {
            for (int i = 0; i < count; i++)
            {
                bool isPlaced = false;  // Renaming 'placed' to 'isPlaced' to avoid scope conflict
                while (!isPlaced)
                {
                    Console.WriteLine($"Place your {shipSize}-mast ship (Ship {i + 1} of {count}).");
                    Console.WriteLine("Enter the start coordinate (x1 y1): ");
                    int x1, y1, x2, y2;
                    string[] startCoord = Console.ReadLine().Split();
                    x1 = int.Parse(startCoord[0]);
                    y1 = int.Parse(startCoord[1]);

                    Console.WriteLine("Enter the end coordinate (x2 y2): ");
                    string[] endCoord = Console.ReadLine().Split();
                    x2 = int.Parse(endCoord[0]);
                    y2 = int.Parse(endCoord[1]);

                    // Calculate the length of the ship
                    int length = (x1 == x2) ? Math.Abs(y2 - y1) + 1 : (y1 == y2) ? Math.Abs(x2 - x1) + 1 : 0;

                    if (length != shipSize)
                    {
                        Console.WriteLine($"The length of the ship doesn't match the required size. The ship size is {shipSize}.");
                    }
                    else
                    {
                        // Create the ship (no need for orientation anymore)
                        IShip ship = CreateShip(player, shipSize);

                        // Place the ship using start and end coordinates
                        isPlaced = player.Board.PlaceShip(ship, x1, y1, x2, y2);

                        if (!isPlaced)
                        {
                            Console.WriteLine("Invalid placement. The ship cannot be placed here.");
                        }
                    }

                    player.Board.DisplayBoard(); // Show the updated board after placing this ship
                }
            }
        }



        static IShip CreateShip(Player player, int shipSize)
        {
            ShipFactory shipFactory = shipSize switch
            {
                1 => OneMastFactory.Instance,
                2 => TwoMastFactory.Instance,
                3 => ThreeMastFactory.Instance,
                4 => FourMastFactory.Instance, // Ensure the correct factory is called for the four-mast ship
                _ => throw new ArgumentException("Invalid ship size")
            };

            return shipFactory.CreateShip(player.PlayerId); // Create the ship through the factory
        }

        static void Attack()
        {

        }
    }
}
