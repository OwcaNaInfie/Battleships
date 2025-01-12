using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleships.Models.Cells;

namespace Battleships.Models
{
    public class Board
    {
        private List<List<Cell>> grid;
        public int size { get; set; }

        public Board(int size = 10)
        {
            this.size = size;

            grid = new();

            for (int i = 0; i < size; i++)
            {
                grid[i] = new List<Cell>();

                for (int j = 0; j < size; j++)
                {
                    grid[i].Add(new Cell());
                }
            }
        }

        //public void PlaceShip(Cell cell, Ship ship, Orientation orientation);
    }
}
