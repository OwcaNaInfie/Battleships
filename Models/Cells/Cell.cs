using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Models.Cells
{
    public class Cell
    {
        public ICellState State { get; set; }
        public string StateName { get; set; }
        public Cell()
        {
            State = new UnattackedEmptyState();
            StateName = State.ToString();

        }
        public Cell(ICellState State)
        {
            this.State = State;

        }

        public void MarkHit()
        {
            State.MarkHit(this);
        }

        public void MarkOccupied()
        {
            State.MarkOccupied(this);
        }

        public string GetState()
        {
            return "State: " + State.ToString();
        }
    }
}
