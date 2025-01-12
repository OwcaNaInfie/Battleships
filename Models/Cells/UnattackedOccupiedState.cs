using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Models.Cells
{
    internal class UnattackedOccupiedState : ICellState
    {
        public void MarkHit(Cell context)
        {
            Console.WriteLine("A ship has been hit!");
            context.state = new HitOccupiedState();
        }

        public void MarkOccupied(Cell context)
        {
            Console.WriteLine("A ship is already placed here.");
        }
    }
}
