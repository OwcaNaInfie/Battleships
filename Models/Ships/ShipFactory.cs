using Battleships.Models.Cells;
using Battleships.Models.Ships;

namespace Battleships.Models.Ships
{
    // Abstrakcyjna klasa fabryki statków
    public abstract class ShipFactory
    {
        // Abstrakcyjna metoda do tworzenia statku
        public abstract IShip CreateShip(int playerNumber);

        // Słownik przechowujący liczbę utworzonych statków każdego typu
        private static Dictionary<string, int> ShipCounts = new Dictionary<string, int>
        {
            { "One-Mast", 0 },
            { "Two-Mast", 0 },
            { "Three-Mast", 0 },
            { "Four-Mast", 0 }
        };

        // Słownik przechowujący limity statków każdego typu
        private static Dictionary<string, int> ShipLimits = new Dictionary<string, int>
        {
            { "One-Mast", 4 },
            { "Two-Mast", 3 },
            { "Three-Mast", 2 },
            { "Four-Mast", 1 }
        };

        // Metoda sprawdzająca, czy można utworzyć statek danego typu
        protected bool CanCreateShip(string shipType)
        {
            return ShipCounts[shipType] < ShipLimits[shipType];
        }

        // Metoda inkrementująca liczbę utworzonych statków danego typu
        protected void IncrementShipCount(string shipType)
        {
            ShipCounts[shipType]++;
        }

        // Metoda dekorująca statek w zależności od numeru gracza
        protected IShip DecorateShip(IShip ship, int playerNumber)
        {
            return playerNumber == 1 ? new Player1ShipDecorator(ship) : new Player2ShipDecorator(ship);
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
        public virtual string Pattern { get => DecoratedShip.Pattern; set => DecoratedShip.Pattern = value; }
        public virtual string Color { get => DecoratedShip.Color; set => DecoratedShip.Color = value; }
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
    }

    // Dekorator statków dla gracza 1
    public class Player1ShipDecorator : ShipDecorator
    {
        public Player1ShipDecorator(IShip ship) : base(ship)
        {
            DecoratedShip.Pattern = "Blocky";
            DecoratedShip.Color = "Red";
        }

        public override void ChangeTheme()
        {
            DecoratedShip.Pattern = "Round";
            DecoratedShip.Color = "Green";
            Console.WriteLine("Theme changed.");
        }
    }

    // Dekorator statków dla gracza 2
    public class Player2ShipDecorator : ShipDecorator
    {
        public Player2ShipDecorator(IShip ship) : base(ship)
        {
            DecoratedShip.Pattern = "Round";
            DecoratedShip.Color = "Blue";
        }

        public override void ChangeTheme()
        {
            DecoratedShip.Pattern = "Blocky";
            DecoratedShip.Color = "Purple";
            Console.WriteLine("Theme changed.");
        }
    }

    // Fabryka statków jednomasztowych
    public class OneMastFactory : ShipFactory
    {
        private static OneMastFactory instance;
        public static OneMastFactory Instance => instance ??= new OneMastFactory();

        public OneMastFactory() { }

        public override IShip CreateShip(int playerNumber)
        {
            if (!CanCreateShip("One-Mast"))
                throw new InvalidOperationException("Nie można stworzyć wiekszej ilości jednomasztowców.");
            IncrementShipCount("One-Mast");
            IShip ship = new Ship { Name = "One-Mast", Size = 1 };
            return DecorateShip(ship, playerNumber);
        }
    }

    // Fabryka statków dwumasztowych
    public class TwoMastFactory : ShipFactory
    {
        private static TwoMastFactory instance;
        public static TwoMastFactory Instance => instance ??= new TwoMastFactory();

        public TwoMastFactory() { }

        public override IShip CreateShip(int playerNumber)
        {
            if (!CanCreateShip("Two-Mast"))
                throw new InvalidOperationException("Nie można stworzyć wiekszej ilości dwumasztowców.");
            IncrementShipCount("Two-Mast");
            IShip ship = new Ship { Name = "Two-Mast", Size = 2 };
            return DecorateShip(ship, playerNumber);
        }
    }

    // Fabryka statków trójmasztowych
    public class ThreeMastFactory : ShipFactory
    {
        private static ThreeMastFactory instance;
        public static ThreeMastFactory Instance => instance ??= new ThreeMastFactory();

        public ThreeMastFactory() { }

        public override IShip CreateShip(int playerNumber)
        {
            if (!CanCreateShip("Three-Mast"))
                throw new InvalidOperationException("Nie można stworzyć wiekszej ilości trójmasztowców.");
            IncrementShipCount("Three-Mast");
            IShip ship = new Ship { Name = "Three-Mast", Size = 3 };
            return DecorateShip(ship, playerNumber);
        }
    }

    // Fabryka statków czteromasztowych
    public class FourMastFactory : ShipFactory
    {
        private static FourMastFactory instance;
        public static FourMastFactory Instance => instance ??= new FourMastFactory();

        public FourMastFactory() { }

        public override IShip CreateShip(int playerNumber)
        {
            if (!CanCreateShip("Four-Mast"))
                throw new InvalidOperationException("Nie można stworzyć wiekszej ilości czteromasztowców.");
            IncrementShipCount("Four-Mast");
            IShip ship = new Ship { Name = "Four-Mast", Size = 4 };
            return DecorateShip(ship, playerNumber);
        }
    }

    // Klasa reprezentująca statek
    public class Ship : IShip
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public List<Cell> Positions { get; set; }
        public int Hits { get; set; }
        public string Pattern { get; set; }
        public string Color { get; set; }
        public bool IsSunk()
        {
            return Hits >= Size;
        }
        public void ChangeTheme()
        {

        }
    }

}