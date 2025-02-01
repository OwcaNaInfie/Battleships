using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Cells
{
    internal class UnattackedEmptyState : ICellState
    {
        public string StateDescription = "UnattackedEmptyState";

        public bool MarkHit(Cell context)
        {
            Console.WriteLine("It's a miss...");
            context.State = new HitEmptyState();
            StateDescription = "UnattackedEmptyState";
            return true;
        }

        public void MarkOccupied(Cell context)
        {
            Console.WriteLine("A ship has been placed.");
            context.State = new UnattackedOccupiedState();
            StateDescription = "UnattackedOccupiedState";
        }

        public override string ToString()
        {
            return StateDescription;
        }
    }
}
