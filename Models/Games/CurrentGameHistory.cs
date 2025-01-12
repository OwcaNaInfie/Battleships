using Battleships.Models.Games;

namespace Battleships.Models.Games
{
    public class CurrentGameHistory
    {
        // List of all the states in a current game
        private readonly Stack<IGameState> gameHistory = new Stack <IGameState>();

        public void Push(IGameState iGameState) 
        {
            gameHistory.Push(iGameState);
        }

        public IGameState Pop()
        {
            if(gameHistory.Count == 0)
            {
                Console.WriteLine("No states to restore");
                return null;
            }
            return gameHistory.Pop();
        }
    }
}