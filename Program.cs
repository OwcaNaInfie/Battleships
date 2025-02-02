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
                DisplayBoards(game, '.');
                OptionsChoice(game);
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

            PlaceShipForPlayer(game, player, 4, 3, 2, 1);
            game.CommandInvoker.ClearHistory();


            Console.WriteLine($"{player.Name} has placed all ships.");
        }

        static void FillSizeArray(List<int> sizeArray, int oneMast, int twoMast, int threeMast, int fourMast)
        {

            for (int i = 0; i < oneMast; i++)
            {
                sizeArray.Add(1);
            }

            for (int i = 0; i < twoMast; i++)
            {
                sizeArray.Add(2);
            }

            for (int i = 0; i < threeMast; i++)
            {
                sizeArray.Add(3);
            }

            for (int i = 0; i < fourMast; i++)
            {
                sizeArray.Add(4);
            }
        }

        static int CalculateShipsNow(List<int> sizeArray, int shipCount)
        {
            int lastIndex = 0;

            if (sizeArray[shipCount] != 1)
            {
                lastIndex = sizeArray.FindLastIndex(x => x == sizeArray[shipCount] - 1) + 1;
            }

            return shipCount - lastIndex;
        }

        static void PlaceShipForPlayer(Game game, Player player, int oneMast, int twoMast, int threeMast, int fourMast)
        {
            int sum = oneMast + twoMast + threeMast + fourMast;
            int shipCount = 0;

            List<int> sizeArray = new();
            FillSizeArray(sizeArray, oneMast, twoMast, threeMast, fourMast);

            Dictionary<int, int> shipTypeCount = new Dictionary<int, int>
            {
                { 1, oneMast },
                { 2, twoMast },
                { 3, threeMast },
                { 4, fourMast }
            };


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
                            int shipSize = sizeArray[shipCount];
                            int totalOfShipType = shipTypeCount[shipSize];
                            int shipTypeNowCount = CalculateShipsNow(sizeArray, shipCount);

                            success = PlaceShip(shipSize, totalOfShipType, shipTypeNowCount, player, game);
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
            bool isPlaced = false;
            while (!isPlaced)
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

                    // Obliczenie długości statku
                    int length = (x1 == x2) ? Math.Abs(y2 - y1) + 1 : (y1 == y2) ? Math.Abs(x2 - x1) + 1 : 0;
                    if (length != shipSize)
                    {
                        Console.WriteLine($"The length of the ship doesn't match the required size. The ship size is {shipSize}.");
                        return false;
                    }
                }
                else
                {
                    // Dla 1-masztowców pary współrzędnych są takie same
                    x2 = x1;
                    y2 = y1;
                }

                // Ułożenie statku na planszy
                Models.Commands.ICommand placeShip = new PlaceShipCommand(player.Board, player, shipSize, x1, y1, x2, y2);
                isPlaced = game.CommandInvoker.ExecuteCommand(placeShip);
                player.Board.DisplayBoard(true, '#');//, ship.Symbol);

                if (!isPlaced)
                {
                    Console.WriteLine("Invalid placement. The ship cannot be placed here.");
                    return false;
                }
            }
            return true;
        }

    }
}
