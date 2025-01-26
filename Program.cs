using System;
using Battleships.Models;
using Battleships.Models.Ships;
using Battleships.Models.Games;
using System.Windows.Input;
using Battleships.Models.Commands;
using System.Numerics;

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

            // Start the game
            game.StartGame();

            // Let each player place their ships
            PlaceShips(game.Player1, game);
            PlaceShips(game.Player2, game);

            game.CommandInvoker.ClearHistory();

            // Start the game loop
            while (game.CheckWinner() == null)
            {
                Console.WriteLine(game.Status);
                DisplayBoards(game);
                OptionsChoice(game);
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

        static void PlaceShips(Player player, Game game)
        {
            Console.WriteLine($"{player.Name}, it's time to place your ships!\n");

            player.Board.DisplayBoard();

            // Corrected number of ships
            PlaceShipForPlayer(game, player, 1, 4); // 4 ships of size 1-mast
            PlaceShipForPlayer(game, player, 2, 3); // 3 ships of size 2-mast
            PlaceShipForPlayer(game, player, 3, 2); // 2 ships of size 3-mast
            PlaceShipForPlayer(game, player, 4, 1); // 1 ship of size 4-mast

            Console.WriteLine($"{player.Name} has placed all ships.");
        }

        static void PlaceShipForPlayer(Game game, Player player, int shipSize, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(i);
                Console.WriteLine("Select action: P - place ship, U - undo, R - redo");
                string? input;
                bool inputInvalid = true;
                bool success;

                do
                {
                    input = Console.ReadLine();
                    switch (input)
                    {
                        case "P" or "p":
                            success = PlaceShip(shipSize, count, i, player, game);
                            if (success)
                            {
                                inputInvalid = false;
                            }
                            break;
                        case "U" or "u":
                            success = game.CommandInvoker.Undo();
                            if (success)
                            {
                                inputInvalid = false;
                                if (i > 0) i-=2;
                                CancelShipPlacement(player, shipSize);
                            }
                            else
                            {
                                Console.WriteLine("No moves to undo.");
                            }
                            break;
                        case "R" or "r":
                            success = game.CommandInvoker.Redo();
                            if (success)
                            {
                                inputInvalid = false;
                            }
                            else
                            {
                                Console.WriteLine("No moves to redo.");
                            }
                            break;
                        default:
                            Console.WriteLine("Invalid input. Try again.");
                            inputInvalid = true;
                            break;

                    }

                } while (inputInvalid);
                player.Board.DisplayBoard(true);
            }
            game.CommandInvoker.ClearHistory();
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

        static void CancelShipPlacement(Player player, int shipSize)
        {
            ShipFactory shipFactory = shipSize switch
            {
                1 => OneMastFactory.Instance,
                2 => TwoMastFactory.Instance,
                3 => ThreeMastFactory.Instance,
                4 => FourMastFactory.Instance, // Ensure the correct factory is called for the four-mast ship
                _ => throw new ArgumentException("Invalid ship size")
            };
            shipFactory.CancelShipPlacement(player.PlayerId);
        }

        static void Attack(Game game)
        {
            Console.WriteLine("Which cell would you like to attack?");

            Console.WriteLine("Enter x, y coordinates:");
            string[] attackCoord = Console.ReadLine().Split();
            int x = int.Parse(attackCoord[0]);
            int y = int.Parse(attackCoord[1]);

            Models.Commands.ICommand attack = new MarkHitCommand(game.GetOpponentBoard(), x, y);
            bool success = game.CommandInvoker.ExecuteCommand(attack);

            while (!success) 
            {
                Console.WriteLine("Invalid operation. Try again.");

                Console.WriteLine("Enter x, y coordinates:");
                attackCoord = Console.ReadLine().Split();
                x = int.Parse(attackCoord[0]);
                y = int.Parse(attackCoord[1]);

                attack = new MarkHitCommand(game.GetOpponentBoard(), x, y);
                success = game.CommandInvoker.ExecuteCommand(attack);
            }

        }

        private static void DisplayBoards(Game game)
        {
            Console.WriteLine("Your ships' status:");
            game.GetCurrentBoard().DisplayBoard(true);

            Console.WriteLine("Attacks status:");
            Board opponentBoard = game.GetOpponentBoard();
            opponentBoard.DisplayBoard(false);
        }

        private static void OptionsChoice(Game game)
        {
            Console.WriteLine("Select action: A - attack, U - undo, R - redo");
            string? input;
            bool inputInvalid = true;
            bool success;

            do
            {
                input = Console.ReadLine();
                switch (input)
                {
                    case "A" or "a":
                        Attack(game);
                        inputInvalid = false;
                        break;
                    case "U" or "u":
                        success = game.CommandInvoker.Undo();
                        if (success)
                        {
                            inputInvalid = false;
                        }
                        else
                        {
                            Console.WriteLine("No moves to undo.");
                        }
                        break;
                    case "R" or "r":
                        success = game.CommandInvoker.Redo();
                        if (success)
                        {
                            inputInvalid = false;
                        }
                        else
                        {
                            Console.WriteLine("No moves to redo.");
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid input. Try again.");
                        inputInvalid = true;
                        break;

                }

            } while (inputInvalid);

            game.SwitchTurn();
        }

        private static bool PlaceShip(int shipSize, int count, int i, Player player, Game game)
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
                    return false;
                }
                else
                {
                    // Create the ship (no need for orientation anymore)
                    IShip ship = CreateShip(player, shipSize);

                    // Place the ship using start and end coordinates
                    //isPlaced = player.Board.PlaceShip(ship, x1, y1, x2, y2);
                    Models.Commands.ICommand placeShip = new PlaceShipCommand(player.Board, ship, x1, y1, x2, y2);
                    isPlaced = game.CommandInvoker.ExecuteCommand(placeShip);

                    if (!isPlaced)
                    {
                        Console.WriteLine("Invalid placement. The ship cannot be placed here.");
                        CancelShipPlacement(player, shipSize);
                        return false;
                    }
                }

            }
            return true;
        }
    }
}
