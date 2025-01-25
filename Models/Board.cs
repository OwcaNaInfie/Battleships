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

        //public void PlaceShip(Cell cell, Ship ship, Orientation orientation);
    }
}
