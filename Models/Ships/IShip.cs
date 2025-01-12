using Battleships.Models.Cells;
namespace Battleships.Models.Ships
{
    public interface IShip
    {
        string Name { get; set; }
        int Size { get; set; }

        List<Cell> Positions { get; set; }
        int Hits { get; set; }
        bool IsSunk();
        void ChangeTheme();
    }
 
}
