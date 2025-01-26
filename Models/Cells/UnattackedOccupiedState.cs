using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Models.Cells
{
    internal class UnattackedOccupiedState : ICellState
    {
        public bool MarkHit(Cell context)
        {
            Console.WriteLine("A ship has been hit!");
            context.State = new HitOccupiedState();
            return true;
        }

        public void MarkOccupied(Cell context)
        {
            Console.WriteLine("A ship has already been placed here.");
        }
        public string ToString()
        {
            return "UnattackedOccupiedState";
        }
    }
}
