using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleships.Models.Cells;
using Battleships.Models.Ships;

namespace Battleships.Models
{
    public class Board
    {
      
        public int Size { get; set; }
        public List<List<Cell>> Grid { get; set; }

        public Board(int size = 10)
        {
            this.Size = size;

            Grid = new List<List<Cell>>();

            for (int i = 0; i < size; i++)
            {
                Grid.Add(new List<Cell>());

                for (int j = 0; j < size; j++)
                {
                    Grid[i].Add(new Cell());
                }
            }
        }

        public bool PlaceShip(IShip ship, int startX, int startY, Orientation orientation)
        {
            List<Cell> cells = new List<Cell>();

            for (int i = 0; i < ship.Size; i++)
            {
                int x = startX;
                int y = startY;

                if (orientation == Orientation.Horizontal)
                    x = startX + i;
                else if (orientation == Orientation.Vertical)
                    y = startY + i;

                // Check for valid position
                if (x >= Size || y >= Size || Grid[x][y].State is UnattackedOccupiedState)
                {
                    return false; // Invalid placement
                }

                cells.Add(Grid[x][y]);
            }

            // Place the ship on the cells
            ship.Positions = cells;
            foreach (var cell in cells)
            {
                cell.MarkOccupied();
            }

            return true;
        }
    }

    public enum Orientation
    {
        Horizontal,
        Vertical
    }
}
