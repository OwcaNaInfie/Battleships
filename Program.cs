using System;
using Battleships;
using Battleships.Ships;
using Battleships.Players;
using Battleships.Games;
using System.Windows.Input;
using Battleships.Commands;
using System.Numerics;
using System.Reflection;

namespace Battleships
{
    class Program
    {
        internal static int player1SymbolOption;
        internal static int player2SymbolOption;

        static CurrentGameHistory history = new CurrentGameHistory();
        
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
            Game game = new Game(player1Name, player2Name);

            game.StartGame();

            // Każdy gracz stawia swoje statki
            PlaceShips(game.Player1, game);
            game.CommandInvoker.ClearHistory();
            PlaceShips(game.Player2, game);
            game.CommandInvoker.ClearHistory();

            // Rozpoczęcie pętli gry
            while (game.CheckWinner() == null)
            {
                game.Status = $"\n{game.CurrentTurn.Name}'s turn";
                Console.WriteLine(game.Status);

                DisplayBoards(game, game.CurrentTurn);
                OptionsChoice(game);
            }

            // Powiadom o wygranym graczu
            Player winner = game.CheckWinner();
            if (winner != null)
            {
                game.Status = $"\n{winner.Name} WON";
                history.Push(game.Save());
                
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


        static void PlaceShips(Player player, Game game)
        {
            Console.WriteLine($"{player.Name}, it's time to place your ships!\n");

            player.Board.DisplayBoard(true, '.');

            Dictionary<int, int> shipQuantities = DefineShipQuantities();

            PlaceShipForPlayer(game, player, shipQuantities);


            Console.WriteLine($"{player.Name} has placed all ships.");
        }


        static Dictionary<int, int> DefineShipQuantities()
        {
            // Licznosci statkow danego typu
            int oneMast = 4;
            int twoMast = 3;
            int threeMast = 2;
            int fourMast = 1;

            return new Dictionary<int, int>
            {
                { 1, oneMast },
                { 2, twoMast },
                { 3, threeMast },
                { 4, fourMast }
            };
        }

        // Metoda pozwalająca graczowi wykonać akcję podczas stawiania statków
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
                            int shipTotal = shipQuantities[shipSize];
                            int shipRelativeCount = FindRelativeShipCount(shipQuantities, shipCount);

                            success = PlaceShip(shipSize, shipTotal, shipRelativeCount, player, game);
                            //player.Board.DisplayBoard(true, '#');
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

            Commands.ICommand attack = new MarkHitCommand(game.GetOpponentBoard(), x, y);
            bool success = game.CommandInvoker.ExecuteCommand(attack);

            if (!success)
            {
                Console.WriteLine("Invalid operation. Try again.");
                Attack(game); 
            }
            else
            {
                history.Push(game.Save());
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
            Console.WriteLine("Select action: A - attack, U - undo, R - redo, H - view history");
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
                    case "H" or "h":
                        DisplayGameHistory();
                        Console.WriteLine("Select action: A - attack, U - undo, R - redo, H - view history");
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
            Commands.ICommand placeShip = new PlaceShipCommand(player, shipSize, x1, y1, x2, y2);
            return game.CommandInvoker.ExecuteCommand(placeShip);

        }


        // Metoda pokazująca historię aktualnej rozgrywki
        private static void DisplayGameHistory()
        {
            Console.WriteLine("Game History:");

            var historyList = history.GetHistory();
            if (historyList.Count == 0)
            {
                Console.WriteLine("No game history available.");
                return;
            }

            foreach (var state in historyList)
            {
                Console.WriteLine(state.ToString());
            }
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

    }
}
