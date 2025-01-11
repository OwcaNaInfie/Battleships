using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZespolowyStateCommand.Cells
{
    internal class UnattackedEmptyState : ICellState
    {
        public void MarkHit(Cell context)
        {
            Console.WriteLine("It's a miss...");
            context.state = new HitEmptyState();
        }

        public void MarkOccupied(Cell context)
        {
            Console.WriteLine("A ship has been placed.");
            context.state = new UnattackedOccupiedState();
        }
    }
}
