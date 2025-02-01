using Battleships.Cells;
namespace Battleships.Ships
{
    public interface IShip
    {
        string Name { get; set; }
        int Size { get; set; }
        char Symbol { get; set; }
        List<Cell> Positions { get; set; }
        int Hits { get; set; }
        bool IsSunk();
        void ChangeTheme();
        IShip Clone();
    }

}
