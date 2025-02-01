using Battleships.Models.Cells;
using Battleships.Models.Ships;
using Battleships.Models;

namespace Battleships.Models.Ships
{
    // Abstrakcyjna klasa fabryki statków
    public abstract class ShipFactory
    {
        // Abstrakcyjna metoda do tworzenia statku
        public abstract IShip CreateShip(int playerId);
        public abstract void CancelShipPlacement(int playerId);

        private static Dictionary<int, Dictionary<string, int>> PlayerShipCounts = new();

        // Słownik przechowujący limity statków każdego typu
        private static Dictionary<string, int> ShipLimits = new Dictionary<string, int>
        {
            { "One-Mast", 4 },
            { "Two-Mast", 3 },
            { "Three-Mast", 2 },
            { "Four-Mast", 1 }
        };

        // Metoda sprawdzająca, czy można utworzyć statek danego typu
        protected bool CanCreateShip(string shipType, int playerId)
        {
            if (!PlayerShipCounts.ContainsKey(playerId))
            {
                Dictionary<string, int> newPlayerDictionary = new Dictionary<string, int>
                {
                    { "One-Mast", 0 },
                    { "Two-Mast", 0 },
                    { "Three-Mast", 0 },
                    { "Four-Mast", 0 }
                };

                PlayerShipCounts.Add(playerId, newPlayerDictionary);
            }

            return PlayerShipCounts[playerId][shipType] < ShipLimits[shipType];
        }

        // Metoda inkrementująca liczbę utworzonych statków danego typu
        protected void IncrementShipCount(string shipType, int playerId)
        {
            PlayerShipCounts[playerId][shipType]++;
        }

        protected void DecrementShipCount(string shipType, int playerId)
        {
            if (PlayerShipCounts[playerId][shipType] > 0)
            {
                PlayerShipCounts[playerId][shipType]--;
            }
        }

        // Metoda dekorująca statek w zależności od numeru gracza
        protected IShip DecorateShip(IShip ship, int playerId)
        {
            return playerId == 1 ? new Player1ShipDecorator(ship, playerId) : new Player2ShipDecorator(ship, playerId);
        }
    }

    // Abstrakcyjna klasa dekoratora statków
    public abstract class ShipDecorator : IShip
    {
        protected IShip DecoratedShip;

        public ShipDecorator(IShip ship)
        {
            DecoratedShip = ship;
        }
        public virtual string Name { get => DecoratedShip.Name; set => DecoratedShip.Name = value; }
        public virtual int Size { get => DecoratedShip.Size; set => DecoratedShip.Size = value; }
        public virtual char Symbol { get => DecoratedShip.Symbol; set => DecoratedShip.Symbol = value; }

        public virtual List<Cell> Positions { get => DecoratedShip.Positions; set => DecoratedShip.Positions = value; }
        public virtual int Hits { get => DecoratedShip.Hits; set => DecoratedShip.Hits = value; }

        public virtual bool IsSunk()
        {
            return DecoratedShip.IsSunk();
        }

        public virtual void ChangeTheme()
        {
            DecoratedShip.ChangeTheme();
        }

        public virtual IShip Clone()
        {
            return DecoratedShip.Clone();
        }
    }
   
    // Dekorator statków dla gracza 1
    public class Player1ShipDecorator : ShipDecorator
    {
        public Player1ShipDecorator(IShip ship, int playerId) : base(ship)
        {
            var hallOfFame = HallOfFame.GetInstance();
            var playerStats = hallOfFame.GetPlayerStatistics().FirstOrDefault(stats => stats.PlayerID == playerId);

            if (playerStats != null && playerStats.GamesWon >= 3)
            {
                DecoratedShip.Symbol = '!';
            }
            else
            {
                DecoratedShip.Symbol = '@';
            }
        }
    }

    // Dekorator statków dla gracza 2
    public class Player2ShipDecorator : ShipDecorator
    {
        public Player2ShipDecorator(IShip ship, int playerId) : base(ship)
        {
            var hallOfFame = HallOfFame.GetInstance();
            var playerStats = hallOfFame.GetPlayerStatistics().FirstOrDefault(stats => stats.PlayerID == playerId);

            if (playerStats != null && playerStats.GamesWon >= 3)
            {
                DecoratedShip.Symbol = '+';
            }
            else
            {
                DecoratedShip.Symbol = '&';
            }
        }
    }

    // Fabryka statków jednomasztowych
    public class OneMastFactory : ShipFactory
    {
        private static OneMastFactory instance;
        public static OneMastFactory Instance => instance ??= new OneMastFactory();

        public OneMastFactory() { }

        public override IShip CreateShip(int playerId)
        {
            if (!CanCreateShip("One-Mast", playerId))
            {
                Console.WriteLine("Nie można stworzyć wiekszej ilości jednomasztowców.");
            }
            IncrementShipCount("One-Mast", playerId);
            IShip ship = new Ship { Name = "One-Mast", Size = 1 };
            return DecorateShip(ship, playerId);
        }

        public override void CancelShipPlacement(int playerId)
        {
            DecrementShipCount("One-Mast", playerId);
        }
    }

    // Fabryka statków dwumasztowych
    public class TwoMastFactory : ShipFactory
    {
        private static TwoMastFactory instance;
        public static TwoMastFactory Instance => instance ??= new TwoMastFactory();

        public TwoMastFactory() { }

        public override IShip CreateShip(int playerId)
        {
            if (!CanCreateShip("Two-Mast", playerId))
            {
                Console.WriteLine("Nie można stworzyć wiekszej ilości dwumasztowców.");
            }
            IncrementShipCount("Two-Mast", playerId);
            IShip ship = new Ship { Name = "Two-Mast", Size = 2 };
            return DecorateShip(ship, playerId);
        }
        public override void CancelShipPlacement(int playerId)
        {
            DecrementShipCount("Two-Mast", playerId);
        }
    }

    // Fabryka statków trójmasztowych
    public class ThreeMastFactory : ShipFactory
    {
        private static ThreeMastFactory instance;
        public static ThreeMastFactory Instance => instance ??= new ThreeMastFactory();

        public ThreeMastFactory() { }

        public override IShip CreateShip(int playerId)
        {
            if (!CanCreateShip("Three-Mast", playerId))
            {
                Console.WriteLine("Nie można stworzyć wiekszej ilości trójmasztowców.");
            }
            IncrementShipCount("Three-Mast", playerId);
            IShip ship = new Ship { Name = "Three-Mast", Size = 3 };
            return DecorateShip(ship, playerId);
        }
        public override void CancelShipPlacement(int playerId)
        {
            DecrementShipCount("Three-Mast", playerId);
        }
    }

    // Fabryka statków czteromasztowych
    public class FourMastFactory : ShipFactory
    {
        private static FourMastFactory instance;
        public static FourMastFactory Instance => instance ??= new FourMastFactory();

        public FourMastFactory() { }

        public override IShip CreateShip(int playerId)
        {
            if (!CanCreateShip("Four-Mast", playerId))
            {
                Console.WriteLine("Nie można stworzyć wiekszej ilości czteromasztowców.");
            }
            IncrementShipCount("Four-Mast", playerId);
            IShip ship = new Ship { Name = "Four-Mast", Size = 4 };
            return DecorateShip(ship, playerId);
        }

        public override void CancelShipPlacement(int playerId)
        {
            DecrementShipCount("Four-Mast", playerId);
        }
    }

    // Klasa reprezentująca statek
    public class Ship : IShip
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public List<Cell> Positions { get; set; }
        public int Hits { get; set; }
        public char Symbol { get; set; }

        public bool IsSunk()
        {
            return Hits >= Size;
        }
        
        public void ChangeTheme()
        {

        }
        
        public IShip Clone()
        {
            return new Ship
            {
                Name = this.Name,
                Size = this.Size,
                Positions = this.Positions,
                Hits = this.Hits,
                Symbol = this.Symbol
            };
        }
    }

}