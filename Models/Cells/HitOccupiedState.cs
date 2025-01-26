using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Models.Cells
{
    internal class HitOccupiedState : ICellState
    {
        public bool MarkHit(Cell context)
        {
            Console.WriteLine("This cell has already been attacked.");
            return false;
        }

        public void MarkOccupied(Cell context)
        {
            Console.WriteLine("Ships' locations can only be changed before the game is underway.");
        }
        public string ToString()
        {
            return "HitOccupiedState";
        }
    }
}
