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


            // Stworzenie gry i inicjalizacja graczy
            Game game = new Game(1, player1Name, player2Name);

            game.StartGame();

            // Każdy gracz stawia swoje statki
            PlaceShips(game.Player1, game);
            PlaceShips(game.Player2, game);

            game.CommandInvoker.ClearHistory();

            // Rozpoczęcie pętli gry
            while (game.CheckWinner() == null)
            {
                Console.WriteLine(game.Status);
                DisplayBoards(game, '#');
                AttackChoices(game);
            }

            // Powiadom o wygranym graczu
            Player winner = game.CheckWinner();
            if (winner != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nCongratulations, {winner.Name}! You are the winner!");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Game over without a winner.");
            }

        }

        static void PlaceShips(Player player, Game game)
        {
            Console.WriteLine($"{player.Name}, it's time to place your ships!\n");

            player.Board.DisplayBoard(true, '.');

            Dictionary<int, int> shipQuantities = DefineShipQuantities();

            PlaceShipForPlayer(game, player, shipQuantities);
            game.CommandInvoker.ClearHistory();


            Console.WriteLine($"{player.Name} has placed all ships.");
        }

        static Dictionary<int, int> DefineShipQuantities()
        {
            // Licznosci statkow danego typu
            int oneMast = 3;
            int twoMast = 2;
            //int threeMast = 2;
            //int fourMast = 1;

            return new Dictionary<int, int>
            {
                { 1, oneMast },
                { 2, twoMast },
                //{ 3, threeMast },
                //{ 4, fourMast }
            };
        }

        // Calculates the total number of ships to be placed
        static int CalculateShipSum(Dictionary<int, int> shipQuantities)
        {
            int sum = 0;

            foreach (KeyValuePair<int, int> pair in shipQuantities)
            {
                sum += pair.Value;
            }

            return sum;
        }

        // Finds the ship size of the n-th ship being placed (n = shipCount)
        static int FindShipSize(Dictionary<int, int> shipQuantities, int shipCount) 
        {
            int sum = 0;

            foreach (KeyValuePair<int, int> pair in shipQuantities)
            {
                sum += pair.Value;
                if (sum > shipCount) return pair.Key;
            }

            return -1;
        }

        // Returns the relative number of the ship being placed
        // e. g. if the ship is the 3rd 2-mast ship to be placed, this method will return 3
        static int FindRelativeShipCount(Dictionary<int, int> shipQuantities, int shipCount)
        {
            int sum = 0;

            foreach (KeyValuePair<int, int> pair in shipQuantities)
            {
                sum += pair.Value;
                if (sum > shipCount) return (shipCount - (sum - pair.Value));
            }

            return -1;
        }

        static void PlaceShipForPlayer(Game game, Player player, Dictionary<int, int> shipQuantities)
        {
            int sum = CalculateShipSum(shipQuantities);

            int shipCount = 0;

            while (shipCount < sum)
            {
                string? input;
                bool inputInvalid = true;
                bool success;

                do
                {
                    Console.WriteLine("Select action: P - place ship, U - undo, R - redo");
                    input = Console.ReadLine();

                    switch (input)
                    {
                        case "P" or "p":

                            int shipSize = FindShipSize(shipQuantities, shipCount);
                            int shipTotal = shipQuantities[ shipSize ];
                            int shipRelativeCount = FindRelativeShipCount(shipQuantities, shipCount);

                            success = PlaceShip(shipSize, shipTotal, shipRelativeCount, player, game);
                            player.Board.DisplayBoard(true, '#');
                            if (success)
                            {
                                inputInvalid = false;
                                shipCount++;
                            }
                            break;
                        case "U" or "u":
                            success = game.CommandInvoker.Undo();
                            if (success)
                            {
                                player.Board.DisplayBoard(true, '#');
                                inputInvalid = false;
                                shipCount--;
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
                                player.Board.DisplayBoard(true, '#');//, ship.Symbol);
                                inputInvalid = false;
                                shipCount++;
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

            } 
        }


        // Metoda wykonująca czynność ataku
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

        private static void DisplayBoards(Game game, char shipSymbol)
        {
            Console.WriteLine("Your ships' status:");
            game.GetCurrentBoard().DisplayBoard(true, shipSymbol); // Plansza gracza z widocznymi statkami

            Console.WriteLine("Attacks status:");
            Board opponentBoard = game.GetOpponentBoard();
            opponentBoard.DisplayBoard(false); // Plansza przeciwnika z widocznymi atakami
        }

        //Metoda wyświetlająca czynności możliwe podczas rozpoczętej rozgrywki
        private static void AttackChoices(Game game)
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

            Console.WriteLine($"Place your {shipSize}-mast ship (Ship {i + 1} of {count}).");
            Console.WriteLine("Enter the start coordinate (x1 y1): ");

            int x1, y1, x2 = 0, y2 = 0;
            string[] startCoord = Console.ReadLine().Split();
            x1 = int.Parse(startCoord[0]);
            y1 = int.Parse(startCoord[1]);

            if (shipSize > 1)
            {
                Console.WriteLine("Enter the end coordinate (x2 y2): ");
                string[] endCoord = Console.ReadLine().Split();
                x2 = int.Parse(endCoord[0]);
                y2 = int.Parse(endCoord[1]);

            }
            else
            {
                // Dla 1-masztowców pary współrzędnych są takie same
                x2 = x1;
                y2 = y1;
            }
            // Ułożenie statku na planszy
            Models.Commands.ICommand placeShip = new PlaceShipCommand(player, shipSize, x1, y1, x2, y2);
            return game.CommandInvoker.ExecuteCommand(placeShip);

        }

    }
}
