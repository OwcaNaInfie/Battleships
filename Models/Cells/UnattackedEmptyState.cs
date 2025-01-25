using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Models.Cells
{
    internal class UnattackedEmptyState : ICellState
    {
        public string StateDescription = "empty";

        public void MarkHit(Cell context)
        {
            Console.WriteLine("It's a miss...");
            context.State = new HitEmptyState();
            StateDescription = "HitEmptyState";
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
