using System;
using System.Collections.Generic;
using System.IO;

namespace Battleships.Games
{

    public class ListOfGames
    {
        public List<Game> Games { get; set; } = new List<Game>();

        public ListOfGames()
        {}
        // Ścieżka do pliku z historią gier
        private string HistoryFilePath = "game_history.txt";

        // Metoda zapisująca dany stan gry
        public void SaveGameHistory(Game game)
        {
            string gameHistory = $"GameId: {game.GameId}, Status: {game.Status}\n" +
                                 $"Player 1: {game.Player1.Name}\n" +
                                 $"Player 2: {game.Player2.Name}\n" +
                                 $"Current Turn: {game.CurrentTurn.Name}\n" +
                                 $"Board 1:\n{game.Board1.GetBoardString(true)}\n" +
                                 $"Board 2:\n{game.Board2.GetBoardString(true)}\n" +
                                 $"-----------------------------\n";
            
            // Dodanie do pliku
            File.AppendAllText(HistoryFilePath, gameHistory);
        }

        // Załadowanie wszystkich gier z pliku
        public List<string> LoadGameHistories()
        {
            List<string> histories = new List<string>();

            if (File.Exists(HistoryFilePath))
            {
                var lines = File.ReadAllLines(HistoryFilePath);
                histories.AddRange(lines);
            }

            return histories;
        }
    }
}
