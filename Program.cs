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
        static int player1SymbolOption;
        static int player2SymbolOption;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Battleships!");
            Console.WriteLine("Please enter the name for Player 1:");
            string player1Name = Console.ReadLine();
            int player1Wins = CheckPlayerWins(player1Name);
            DisplayShipSymbolMenu(player1Wins, 1);

            Console.WriteLine("Please enter the name for Player 2:");
            string player2Name = Console.ReadLine();
            int player2Wins = CheckPlayerWins(player2Name);
            DisplayShipSymbolMenu(player2Wins, 2);


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

                DisplayBoards(game, game.CurrentTurn);
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

        static int CheckPlayerWins(string playerName)
        {
            var hallOfFame = HallOfFame.GetInstance();
            var playerStats = hallOfFame.GetPlayerStatistics().FirstOrDefault(stats => stats.PlayerID == playerName.GetHashCode());
            return playerStats?.GamesWon ?? 0;
        }

        static void DisplayShipSymbolMenu(int wins, int playerId)
        {
            string[] symbols = playerId == 1 ? ["@", "&", "!", "+"] : ["*", "#", "<", ">"];

            Console.WriteLine("Choose your ship symbol:");
            Console.WriteLine($"1. {symbols[0]}");
            Console.WriteLine($"2. {symbols[1]}");
            if (wins >= 3)
            {
                Console.WriteLine($"3. {symbols[2]}");
                Console.WriteLine($"4. {symbols[3]}");
            }
            else
            {
                Console.WriteLine($"3. {symbols[2]} (unlocked after 3 wins)");
                Console.WriteLine($"4. {symbols[3]} (unlocked after 3 wins)");
            }

            string? input;
            bool inputInvalid = true;

            do
            {
                input = Console.ReadLine();
                int symbolOption;
                if (int.TryParse(input, out symbolOption) && symbolOption >= 1 && symbolOption <= 4)
                {
                    if (symbolOption >= 3 && wins < 3)
                    {
                        Console.WriteLine("Invalid input. Try again.");
                    }
                    else
                    {
                        if (playerId == 1) player1SymbolOption = symbolOption;
                        else player2SymbolOption = symbolOption;
                        inputInvalid = false;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Try again.");
                }
            } while (inputInvalid);
        }

        static void SetPlayerSymbol(int playerId, int symbolOption)
        {
            char symbol = playerId == 1 ? Player1ShipDecorator.AvailableSymbols[symbolOption - 1] : Player2ShipDecorator.AvailableSymbols[symbolOption - 1];
            // Implementacja ustawienia symbolu dla gracza
        }

        static void PlaceShips(Player player, Game game)
        {
            Console.WriteLine($"{player.Name}, it's time to place your ships!\n");

            player.Board.DisplayBoard(true, '.');

            PlaceShipForPlayer(game, player, 1, 4);
            PlaceShipForPlayer(game, player, 2, 3);
            PlaceShipForPlayer(game, player, 3, 2);
            PlaceShipForPlayer(game, player, 4, 1);



            Console.WriteLine($"{player.Name} has placed all ships.");
        }

        // Metoda pozwalająca graczowi wykonać akcję podczas stawiania statków
        static void PlaceShipForPlayer(Game game, Player player, int shipSize, int count)
        {
            for (int i = 0; i < count; i++)
            {
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
                                if (i > 0) i -= 2;
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
                4 => FourMastFactory.Instance,
                _ => throw new ArgumentException("Invalid ship size")
            };

            // Stworzenie statku
            IShip ship = shipFactory.CreateShip(player.PlayerId);
            IShip decoratedShip = player.PlayerId == 1 ? new Player1ShipDecorator(ship, player1SymbolOption) : new Player2ShipDecorator(ship, player2SymbolOption);

            return decoratedShip;
        }

        static void CancelShipPlacement(Player player, int shipSize)
        {
            ShipFactory shipFactory = shipSize switch
            {
                1 => OneMastFactory.Instance,
                2 => TwoMastFactory.Instance,
                3 => ThreeMastFactory.Instance,
                4 => FourMastFactory.Instance,
                _ => throw new ArgumentException("Invalid ship size")
            };
            shipFactory.CancelShipPlacement(player.PlayerId);
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

        private static void DisplayBoards(Game game, Player currentPlayer)
        {
            Console.WriteLine("Your ships' status:");
            game.GetCurrentBoard().DisplayBoard(true, currentPlayer.Board.Ships.First().Symbol); // Plansza gracza z widocznymi statkami

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

                // Stworzenie statku
                IShip ship = CreateShip(player, shipSize);

                // Ułożenie statku na planszy
                Models.Commands.ICommand placeShip = new PlaceShipCommand(player.Board, ship, x1, y1, x2, y2);
                isPlaced = game.CommandInvoker.ExecuteCommand(placeShip);
                player.Board.DisplayBoard(true, ship.Symbol);
                if (!isPlaced)
                {
                    Console.WriteLine("Invalid placement. The ship cannot be placed here.");
                    CancelShipPlacement(player, shipSize);
                    return false;
                }
            }
            return true;
        }

    }
}
