using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Models.Cells
{
    internal class UnattackedEmptyState : ICellState
    {
        public string stateDescription = "empty";

        public void MarkHit(Cell context)
        {
            Console.WriteLine("It's a miss...");
            context.state = new HitEmptyState();
            stateDescription = "HitEmptyState";
        }

        public void MarkOccupied(Cell context)
        {
            Console.WriteLine("A ship has been placed.");
            context.state = new UnattackedOccupiedState();
            stateDescription = "UnattackedOccupiedState";
        }

        public override string ToString()
        {
            return stateDescription;
        }
    }
}
