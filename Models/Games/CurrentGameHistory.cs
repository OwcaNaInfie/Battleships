using Battleships.Models.Games;

namespace Battleships.Models.Games
{
    public class CurrentGameHistory
    {
        // List of all the states in a current game
        private readonly Stack<IGameState> GameHistory = new Stack <IGameState>();

        public void Push(IGameState iGameState) 
        {
            GameHistory.Push(iGameState);
        }

        public IGameState Pop()
        {
            if(GameHistory.Count == 0)
            {
                Console.WriteLine("No states to restore");
                return null;
            }
            return GameHistory.Pop();
        }

        public List<IGameState> GetHistory()
        {
            return GameHistory.Reverse().ToList();
        }
    }
}