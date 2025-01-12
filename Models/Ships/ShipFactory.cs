using Battleships.Models;
using Battleships.Models.Cells;
using Battleships.Models.Ships;


namespace Battleships.Models.Ships
{
    public abstract class ShipFactory
    {
        public abstract IShip CreateShip();
    }

    public class OneMastFactory : ShipFactory
    {
        public override IShip CreateShip()
        {
            return new Ship { Name = "One-Mast", Size = 1 };
        }
    }

    public class TwoMastFactory : ShipFactory
    {
        public override IShip CreateShip()
        {
            return new Ship { Name = "Two-Mast", Size = 2 };
        }
    }

    public class ThreeMastFactory : ShipFactory
    {
        public override IShip CreateShip()
        {
            return new Ship { Name = "Three-Mast", Size = 3 };
        }
    }
    public class FourMastFactory : ShipFactory
    {
        public override IShip CreateShip()
        {
            return new Ship { Name = "Four-Mast", Size = 3 };
        }
    }

    public class Ship : IShip
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public List<Cell> Positions { get; set; } = new List<Cell>();
        public int Hits { get; set; }

        public bool IsSunk()
        {
            return Hits >= Size;
        }
        public void ChangeTheme()
        {
            Console.WriteLine("Default theme.");
        }
    }

}
