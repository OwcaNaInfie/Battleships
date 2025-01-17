using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Models.Cells
{
    public class HitEmptyState : ICellState
    {
        public void MarkHit(Cell context)
        {
            Console.WriteLine("This cell has already been attacked.");
        }

        public void MarkOccupied(Cell context)
        {
            Console.WriteLine("Ships' locations can only be changed before the game is underway.");
        }
        public string ToString()
        {
            return "HitEmptyState";
        }
    }
}
