using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleships.Models.Ships;

namespace Battleships.Models.Cells
{
    public class Cell
    {
        public ICellState State { get; set; }
        //public string StateName { get; set; }
        public Cell()
        {
            State = new UnattackedEmptyState();
            //StateName = State.ToString();

        }
        

        public Cell(ICellState state)
        {
            State = state ?? throw new ArgumentNullException(nameof(state), "State cannot be null");
            //StateName = State.ToString();
        }

        public bool MarkHit()
        {
            return State.MarkHit(this);
        }

        public void MarkOccupied()
        {
            State.MarkOccupied(this);
        }

        public void MarkUnattackedEmpty()
        {
            State = new UnattackedEmptyState();
        }

        public string GetState()
        {
            return "State: " + State.ToString();
        }
        
    }
}
